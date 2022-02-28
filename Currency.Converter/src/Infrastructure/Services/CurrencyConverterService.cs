
using System.Collections.Concurrent;
using Currency.Converter.Application.Common.Interfaces;
using QuikGraph;
using QuikGraph.Algorithms;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Currency.Converter.Infrastructure.Services;
public class CurrencyConverterService : ICurrencyConverter
{
    public readonly ConcurrentDictionary<string, double> _rateDic = new ConcurrentDictionary<string, double>();
    public readonly AdjacencyGraph<string, Edge<string>> _currencyGraph = new AdjacencyGraph<string, Edge<string>>();

    private static readonly object LockObj = new();
    private readonly ILogger<CurrencyConverterService> _logger;
    public CurrencyConverterService(ILogger<CurrencyConverterService> logger)
    {
        _logger = logger;
    }

    public void ClearConfiguration()
    {
        _logger.LogInformation("Clear Config Operation Started.");
        _rateDic.Clear();

        lock (LockObj)
        {
            _currencyGraph.Clear();
        }
        _logger.LogInformation("Clear Config Operation Finished.");
    }

    public double Convert(string fromCurrency, string toCurrency, double amount)
    {
        _logger.LogInformation("Currency Convert Operation Started.");
        lock (LockObj)
        {
            fromCurrency = fromCurrency.ToUpper();
            toCurrency = toCurrency.ToUpper();

            Func<Edge<string>, double> graphWeight = e => 1;
            try
            {
                var tryGetPath = _currencyGraph.ShortestPathsDijkstra(graphWeight, fromCurrency);
                double totalRate = 1;
                IEnumerable<Edge<string>> path;
                if (tryGetPath(toCurrency, out path))
                {
                    foreach (var e in path)
                    {
                        string key = e.Source + "_" + e.Target;
                        totalRate *= _rateDic[key];
                    }
                    _logger.LogInformation($"Currency Convert Operation Finished for {fromCurrency}_{toCurrency}.");
                    return totalRate * amount;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"Desired Currency not found. {fromCurrency} to {toCurrency}");
                return -1;
            }
        }
        _logger.LogWarning("Currency Convert Operation Finised with Warning Code:-2.");
        return -1;
    }

    public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
    {
        _logger.LogInformation("Update Configuration is Start.");
        Parallel.ForEach(conversionRates, conversionRate =>
        {

            string fromCurrency = conversionRate.Item1.ToUpper();
            string toCurrency = conversionRate.Item2.ToUpper();
            double rate = conversionRate.Item3;
            string key = fromCurrency + "_" + toCurrency;

            _rateDic.AddOrUpdate(key, rate, (key, oldValue) => rate);

            key = toCurrency + "_" + fromCurrency;
            rate = 1 / rate;
            _rateDic.AddOrUpdate(key, rate, (key, oldValue) => rate);

            lock (LockObj)
            {
                if (_currencyGraph.ContainsEdge(fromCurrency, toCurrency) == false)
                {
                    _currencyGraph.AddVerticesAndEdge(new Edge<string>(fromCurrency, toCurrency));
                    _currencyGraph.AddVerticesAndEdge(new Edge<string>(toCurrency, fromCurrency));
                }
            }
        });
        _logger.LogInformation("Update Configuration was Finished.");
    }
}
