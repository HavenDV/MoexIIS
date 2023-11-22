using System.Net.Http;
using CommunityToolkit.Mvvm.ComponentModel;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MoexIIS.Apps.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    public PlotModel? model;

    private HttpClient HttpClient { get; } = new();
    private MoexIISApi Api { get; }
    private bool IsAuthenticated { get; set; }

    public MainViewModel()
    {
        Api = new MoexIISApi(HttpClient);
    }

    public async Task AuthenticateAsync(CancellationToken cancellationToken = default)
    {
        var username =
            Environment.GetEnvironmentVariable("MOEXIIS_USERNAME") ??
            string.Empty;

        var password =
            Environment.GetEnvironmentVariable("MOEXIIS_PASSWORD") ??
            string.Empty;

        await Api.AuthenticateAsync(username, password, cancellationToken);

        IsAuthenticated = true;
    }
    public class Item
    {
        public DateTime DateTime { get; set; }
        public double Y { get; set; }
    }

    public async Task UpdateModelAsync(CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
        {
            await AuthenticateAsync(cancellationToken);
        }

        var values = await Api.GetOpenPositionsOnFuturesAsync(
            "si",
            iss_json: "extended",
            iss_meta: "off",
            futoi_from: "2022-03-03",
            futoi_till: $"{DateTime.Now:yyyy-MM-dd}",
            cancellationToken: cancellationToken);
        var positions = values.ElementAt(1).Futoi ?? throw new InvalidOperationException("Futoi is null.");
        var yur = positions.ElementAt(0);
        var fiz = positions.ElementAt(1);
        var dates = values.ElementAt(1).FutoiDates ?? throw new InvalidOperationException("FutoiDates is null.");
        var from = dates.ElementAt(0).From;
        var till = dates.ElementAt(0).Till;

        //Console.WriteLine($"Time: {yur.Tradetime}. Group: {yur.Clgroup}. Longs: {yur.Pos_long}/{yur.Pos_long_num}. Shorts: {-yur.Pos_short}/{yur.Pos_short_num}");
        //Console.WriteLine($"Time: {fiz.Tradetime}. Group: {fiz.Clgroup}. Longs: {fiz.Pos_long}/{fiz.Pos_long_num}. Shorts: {-fiz.Pos_short}/{fiz.Pos_short_num}");

        var shorts = new List<Item>(positions.Count / 2);
        var longs = new List<Item>(positions.Count / 2);
        for (var i = 1; i < positions.Count; i += 2)
        {
            var value = positions.ElementAt(i);
            var time = DateTime.Parse($"{value.Tradedate} {value.Tradetime}");

            shorts.Add(new Item
            {
                DateTime = time,
                Y = value.Pos_short_num ?? 0,
            });
            longs.Add(new Item
            {
                DateTime = time,
                Y = value.Pos_long_num ?? 0,
            });
        }

        Model = new PlotModel
        {
            Title = "Long/shorts count",
            Axes =
            {
                new DateTimeAxis
                {
                    Position = AxisPosition.Bottom
                },
                new LinearAxis
                {
                    Position = AxisPosition.Left
                },
            },
            Series =
            {
                new LineSeries
                {
                    Title = "Shorts",
                    DataFieldX = "DateTime",
                    DataFieldY = "Y",
                    MarkerType = MarkerType.Circle,
                    MarkerFill = OxyColors.Red,
                    ItemsSource = shorts,
                },
                new LineSeries
                {
                    Title = "Longs",
                    DataFieldX = "DateTime",
                    DataFieldY = "Y",
                    MarkerType = MarkerType.Circle,
                    MarkerFill = OxyColors.Green,
                    ItemsSource = longs,
                },
            }
        };
    }
}
