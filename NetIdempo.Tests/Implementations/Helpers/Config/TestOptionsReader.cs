using Microsoft.Extensions.Options;
using NetIdempo.Common;
using NetIdempo.Implementations.Helpers.Config;

namespace NetIdempo.Tests.Implementations.Helpers.Config;

public class TestOptionsReader
{
    [Fact]
    public void GetIdempotencyKeyHeader_ShouldReturnAnEmptyStringIfOptionNotAvailable()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions { });
        var optionsReader = new OptionsReader(options);

        // Act
        var result = optionsReader.GetIdempotencyKeyHeader();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetIdempotencyKeyHeader__ShouldReturnTheIdempotencyKeyHeader()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions { IdempotencyKeyHeader = "Idempotency-Key" });
        var optionsReader = new OptionsReader(options);
        
        // Act
        var result = optionsReader.GetIdempotencyKeyHeader();
        
        // Assert
        Assert.Equal("Idempotency-Key", result);
    }
    
    [Fact]
    public void GetServiceKeyByIncomingRequestPath_ShouldReturnEmptyStringIfNoServiceMatchesWithPath()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 30,
            Services = new Dictionary<string, ServiceConfig>
            {
                ["TestApi"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5262",
                    PathPrefix = "testservice1"
                },
                ["TestApi2"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5176",
                    PathPrefix = "testservice2"
                }
            }
        });
        var optionsReader = new OptionsReader(options);
        
        // Act
        var result = optionsReader.GetServiceKeyByIncomingRequestPath("/unknown/path");
        
        // Assert
        Assert.Equal(string.Empty, result);
    }
    
    [Fact]
    public void GetServiceKeyByIncomingRequestPath_ShouldReturnServiceKeyIfPathMatches()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 30,
            Services = new Dictionary<string, ServiceConfig>
            {
                ["TestApi"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5262",
                    PathPrefix = "testservice1"
                },
                ["TestApi2"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5176",
                    PathPrefix = "testservice2"
                }
            }
        });
        var optionsReader = new OptionsReader(options);
        
        // Act
        var result = optionsReader.GetServiceKeyByIncomingRequestPath("/testservice1/some/path");
        
        // Assert
        Assert.Equal("TestApi", result);
    }
    
    [Fact]
    public void GetDestinationServiceBaseUrlFromOptions_ShouldReturnEmptyStringIfServiceKeyNotFound()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 30,
            Services = new Dictionary<string, ServiceConfig>
            {
                ["TestApi"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5262",
                    PathPrefix = "testservice1"
                }
            }
        });
        var optionsReader = new OptionsReader(options);
        
        // Act
        var result = optionsReader.GetDestinationServiceBaseUrlFromOptions("UnknownService");
        
        // Assert
        Assert.Equal(string.Empty, result);
    }
    
    [Fact]
    public void GetDestinationServiceBaseUrlFromOptions_ShouldReturnBaseUrlIfServiceKeyExists()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 30,
            Services = new Dictionary<string, ServiceConfig>
            {
                ["TestApi"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5262",
                    PathPrefix = "testservice1"
                }
            }
        });
        var optionsReader = new OptionsReader(options);
        
        // Act
        var result = optionsReader.GetDestinationServiceBaseUrlFromOptions("TestApi");
        
        // Assert
        Assert.Equal("http://localhost:5262", result);
    }
    
    [Fact]
    public void GetPathPrefixByServiceKey_ShouldReturnEmptyStringIfServiceKeyNotFound()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 30,
            Services = new Dictionary<string, ServiceConfig>
            {
                ["TestApi"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5262",
                    PathPrefix = "testservice1"
                }
            }
        });
        var optionsReader = new OptionsReader(options);
        
        // Act
        var result = optionsReader.GetPathPrefixByServiceKey("UnknownService");
        
        // Assert
        Assert.Equal(string.Empty, result);
    }
    
    [Fact]
    public void GetPathPrefixByServiceKey_ShouldReturnPathPrefixIfServiceKeyExists()
    {
        // Arrange
        var options = Options.Create(new NetIdempoOptions
        {
            IdempotencyKeyHeader = "Idempotency-Key",
            IdempotencyKeyLifetime = 30,
            Services = new Dictionary<string, ServiceConfig>
            {
                ["TestApi"] = new ServiceConfig
                {
                    BaseUrl = "http://localhost:5262",
                    PathPrefix = "testservice1"
                }
            }
        });
        var optionsReader = new OptionsReader(options);
        
        // Act
        var result = optionsReader.GetPathPrefixByServiceKey("TestApi");
        
        // Assert
        Assert.Equal("testservice1", result);
    }
}