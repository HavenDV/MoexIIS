# MoexIIS
C# client library and OpenAPI spec for Moex IIS

## Nuget

[![NuGet](https://img.shields.io/nuget/dt/MoexIIS.svg?style=flat-square&label=MoexIIS)](https://www.nuget.org/packages/MoexIIS/)
```
Install-Package MoexIIS
```

## Usage

```cs
using MoexIIS;

using var client = new HttpClient();
var api = new MoexIISApi(client);

await api.AuthenticateAsync(username, password);

var values = await api.GetOpenPositionsOnFuturesAsync("si", iss_json: "extended", iss_meta: "off");
var positions = values.ElementAt(1).Futoi ?? throw new InvalidOperationException("Futoi is null.");
var yur = positions.ElementAt(0);
var fiz = positions.ElementAt(1);
var dates = values.ElementAt(1).FutoiDates ?? throw new InvalidOperationException("FutoiDates is null.");
var from = dates.ElementAt(0).From;
var till = dates.ElementAt(0).Till;

Console.WriteLine($"Open positions: {from}-{till}");
Console.WriteLine($"Time: {yur.Tradetime}. Group: {yur.Clgroup}. Longs: {yur.Pos_long}/{yur.Pos_long_num}. Shorts: {-yur.Pos_short}/{yur.Pos_short_num}");
Console.WriteLine($"Time: {fiz.Tradetime}. Group: {fiz.Clgroup}. Longs: {fiz.Pos_long}/{fiz.Pos_long_num}. Shorts: {-fiz.Pos_short}/{fiz.Pos_short_num}");
```

## Contacts
* [mail](mailto:havendv@gmail.com)

## Links
Initial OpenAPI spec from:  
https://github.com/Klawru/MoexApiSwagger