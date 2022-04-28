namespace MoexIIS.IntegrationTests;

using static BaseTests;

[TestClass]
public class AnalyticalProductsTests
{
    [TestMethod]
    public async Task GetOpenPositionsOnFutures() => await ApiTestAsync(async (api, cancellationToken) =>
    {
        var response = await api.GetOpenPositionsOnFuturesAsync("si", iss_json: "extended", iss_meta: "off", cancellationToken: cancellationToken);

        _ = response.Should().NotBeNull();

        Console.WriteLine(response.GetPropertiesText());
    });
}
