using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Currency.Converter.Application.Common.Interfaces;
using QuikGraph;
using QuikGraph.Algorithms;

namespace Currency.Converter.Infrastructure.Services;
public class CurrencyConverterService : ICurrencyConverter
{
    readonly ConcurrentDictionary<string, double> _rateDic = new ConcurrentDictionary<string, double>();
    readonly AdjacencyGraph<string, Edge<string>> _currencyGraph = new AdjacencyGraph<string, Edge<string>>();

    private static readonly object LockObj = new();

    public void ClearConfiguration()
    {
        _rateDic.Clear();

        lock (LockObj)
        {
            _currencyGraph.Clear();
        }
    }

    public double Convert(string fromCurrency, string toCurrency, double amount)
    {
        lock (LockObj)
        {
            fromCurrency = fromCurrency.ToUpper();
            toCurrency = toCurrency.ToUpper();
            if (_currencyGraph.ContainsEdge(fromCurrency, toCurrency) == false)
            {
                return -1;
            }

            Func<Edge<string>, double> graphWeight = e => 1;

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
                return totalRate * amount;
            }
        }
        return -2;
    }

    public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
    {
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
    }
}
