
using Currency.Converter.Application.Common.Interfaces;
using Currency.Converter.Domain.Entities;
using MediatR;


namespace Currency.Converter.Application.Currency.Commands.UpdateConfig;

public class UpdateConfigCommand : IRequest
{
    public CurrencyRate[] _currencyRates { get; set; }
    public UpdateConfigCommand(CurrencyRate[] currencyRates)
    {
        _currencyRates = currencyRates;
    }
}

public class UpdateConfigCommandHandler : IRequestHandler<UpdateConfigCommand>
{
    public ICurrencyConverter _currencyConverter { get; }
    public UpdateConfigCommandHandler(ICurrencyConverter currencyConverter)
    {
        _currencyConverter = currencyConverter;
    }

    public async Task<Unit> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
    {
        List<Tuple<string, string, double>> conversionRates = new();
        conversionRates.AddRange(from item in request._currencyRates
                                 select new Tuple<string, string, double>(item._fromCurrency, item._toCurrency, item._rate));

        _currencyConverter.UpdateConfiguration(conversionRates);

        return await Task.FromResult(Unit.Value);
    }
}
