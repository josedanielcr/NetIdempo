# NetIdempo

**NetIdempo** is a lightweight, modular middleware for ASP.NET Core that enables idempotent request handling and request forwarding in API Gateway or Edge Service scenarios.

## Features

* Idempotency key validation and response caching
* Request forwarding to backend services based on path prefixes
* Configurable routing and service timeout options
* Suitable for microservices and multi-service environments

## How It Works

NetIdempo intercepts incoming HTTP requests in your API Gateway. If an idempotency key is present in the request headers, the middleware determines whether the request is:

* A new request (key not found): Forwards it to the appropriate downstream service, caches the response, and returns it to the client.
* A repeated request (key found): Returns the previously cached response without reprocessing it.

Routing to backend services is based on path prefixes defined in the configuration.

## Installation

You can install NetIdempo via NuGet:

    dotnet add package NetIdempo

## Configuration

Add the following section to your `appsettings.json`:

    "NetIdempo": {
      "IdempotencyKeyHeader": "Idempotency-Key",
      "IdempotencyKeyLifetime": 30,
      "ServiceTimeoutSeconds": 10,
      "Services": {
        "TestApi": {
          "BaseUrl": "http://localhost:5262",
          "PathPrefix": "testservice1"
        },
        "TestApi2": {
          "BaseUrl": "http://localhost:5176",
          "PathPrefix": "testservice2"
        }
      }
    }

## Usage

1. Register NetIdempo services in your `Program.cs`
```csharp
    builder.Services.AddDistributedMemoryCache(); // or AddStackExchangeRedisCache
    builder.Services.AddNetIdempo(builder.Configuration);
```
2. Use the middleware in your pipeline
```csharp
    app.UseNetIdempo();
```
## Example Routing

If a request arrives to the gateway as:

    POST /testservice1/api/clients
    Host: https://your-gateway.com
    Idempotency-Key: abc123

And the configuration contains

    "Services": {
      "TestApi": {
        "BaseUrl": "http://localhost:5262",
        "PathPrefix": "testservice1"
      }
    }

NetIdempo will forward the request to

    POST http://localhost:5262/api/clients

It will cache the response using the idempotency key `abc123`.
