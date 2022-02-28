namespace Currency.Converter.Domain.Entities;

public class CurrencyRate
{
    public string _fromCurrency { get; set; }
    public string _toCurrency { get; set; }
    public double _rate { get; set; }
    public CurrencyRate(string fromCurrency, string toCurrency, double rate)
    {
        this._fromCurrency = fromCurrency;
        this._toCurrency = toCurrency;
        this._rate = rate;
    }



}
