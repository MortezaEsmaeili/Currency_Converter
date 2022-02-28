namespace Currency.Converter.Domain.Entities;

public class CurrencyRate
{
    public string _fromCurrency { get; set; }
    public string _toCurrency { get; set; }
    public double _rate { get; set; }
}
