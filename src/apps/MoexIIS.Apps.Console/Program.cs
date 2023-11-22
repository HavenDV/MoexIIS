using MoexIIS;

using var client = new HttpClient();
var api = new MoexIISApi(client);

Console.WriteLine("Enter username:");
var username =
    Environment.GetEnvironmentVariable("MOEXIIS_USERNAME") ??
    Console.ReadLine() ??
    string.Empty;

Console.WriteLine("Enter password:");
var password =
    Environment.GetEnvironmentVariable("MOEXIIS_PASSWORD") ??
    Console.ReadLine() ??
    string.Empty;

await api.AuthenticateAsync(username, password);

Console.WriteLine("Authenticated.");

while (true)
{
    var values = await api.GetOpenPositionsOnFuturesAsync("si", iss_json: "extended", iss_meta: "off");
    var positions = values.ElementAt(1).Futoi ?? throw new InvalidOperationException("Futoi is null.");
    var yur = positions.ElementAt(0);
    var fiz = positions.ElementAt(1);
    var dates = values.ElementAt(1).FutoiDates ?? throw new InvalidOperationException("FutoiDates is null.");
    var from = dates.ElementAt(0).From;
    var till = dates.ElementAt(0).Till;

    Console.Clear();
    Console.WriteLine($"Open positions: {from}-{till}");
    Console.WriteLine($"Time: {yur.Tradetime}. Group: {yur.Clgroup}. Longs: {yur.Pos_long}/{yur.Pos_long_num}. Shorts: {-yur.Pos_short}/{yur.Pos_short_num}");
    Console.WriteLine($"Time: {fiz.Tradetime}. Group: {fiz.Clgroup}. Longs: {fiz.Pos_long}/{fiz.Pos_long_num}. Shorts: {-fiz.Pos_short}/{fiz.Pos_short_num}");

    await Task.Delay(TimeSpan.FromSeconds(5));
}
