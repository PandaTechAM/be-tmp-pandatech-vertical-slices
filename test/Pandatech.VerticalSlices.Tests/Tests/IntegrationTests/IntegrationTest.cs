using Pandatech.VerticalSlices.Tests.Configurations;

namespace Pandatech.VerticalSlices.Tests.Tests.IntegrationTests;

[Collection("Shared Postgres")]
public class IntegrationTest : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetState;

    public IntegrationTest(ApiFactory factory)
    {
        _client = factory.HttpClient;
        _resetState = factory.ResetStateAsync;
    }

    public ValueTask InitializeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _resetState();
    }

    [Fact]
    public void MethodName()
    {
        // Arrange

        // Act

        // Assert
        Assert.True(true);
    }
}
