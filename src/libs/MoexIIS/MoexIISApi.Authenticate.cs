using System.Net.Http.Headers;
using System.Text;

namespace MoexIIS;

/// <summary>
/// Class providing methods for API access.
/// </summary>
public partial class MoexIISApi
{
    /// <summary>
    /// Authenticates using <paramref name="username"/> and <paramref name="password"/>.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    public async Task AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        username = username ?? throw new ArgumentNullException(nameof(username));
        password = password ?? throw new ArgumentNullException(nameof(password));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            scheme: "Basic",
            parameter: Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));

        var defaultBaseUrl = BaseUrl;
        BaseUrl = "https://passport.moex.com/";

        var token = await AuthenticateAsync(cancellationToken).ConfigureAwait(false);

        BaseUrl = defaultBaseUrl;

        _httpClient.DefaultRequestHeaders.Add("MicexPassportCert", token);
    }
}
