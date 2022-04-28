namespace MoexIIS.IntegrationTests;

internal static class BaseTests
{
    public static async Task ApiTestAsync(Func<MoexIISApi, CancellationToken, Task> action)
    {
        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var cancellationToken = source.Token;

        var username =
            Environment.GetEnvironmentVariable("MOEXIIS_USERNAME") ??
            throw new AssertInconclusiveException("MOEXIIS_USERNAME environment variable is not found.");
        var password =
            Environment.GetEnvironmentVariable("MOEXIIS_PASSWORD") ??
            throw new AssertInconclusiveException("MOEXIIS_PASSWORD environment variable is not found.");

        using var client = new HttpClient();
        var api = new MoexIISApi(client);

        await api.AuthenticateAsync(username, password, cancellationToken).ConfigureAwait(false);

        await action(api, cancellationToken).ConfigureAwait(false);
    }
}
