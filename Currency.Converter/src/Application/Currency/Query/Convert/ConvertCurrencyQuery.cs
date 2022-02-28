using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Currency.Converter.Domain.Entities;
using Currency.Converter.Application.Common.Interfaces;

namespace Currency.Converter.Application.Currency.Query.Convert;

public class ConvertCurrencyQuery : IRequest<ConvertResponse>
{
    public string _fromCurrency { get; set; }
    public string _toCurrency { get; set; }
    public double _amount { get; set; }
    public ConvertCurrencyQuery(string fromCurrency, string toCurrency, double amount)
    {
        this._fromCurrency = fromCurrency;
        this._toCurrency = toCurrency;
        this._amount = amount;
    }
}

public class ConvertCurrencyQueryHandler2 : IRequestHandler<ConvertCurrencyQuery, ConvertResponse>
{
    private readonly ICurrencyConverter _currencyConverter;

    public ConvertCurrencyQueryHandler2(ICurrencyConverter currencyConverter)
    {
        _currencyConverter = currencyConverter;
    }
    public Task<ConvertResponse> Handle(ConvertCurrencyQuery request, CancellationToken cancellationToken)
    {
        double convertedValue = _currencyConverter.
            Convert(request._fromCurrency, request._toCurrency, request._amount);
        
        string description = "";
        switch (convertedValue)
        {
            case -1:
                description = "Not Found";
                break;
            case -2:
                description = "InternalError";
                break;
            default:
                description = "Success";
                break;
        }
        ConvertResponse response = new(description, convertedValue);

        return Task.FromResult(response);
    }
}