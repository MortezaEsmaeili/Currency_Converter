
using Currency.Converter.Application.Common.Interfaces;
using MediatR;

namespace Currency.Converter.Application.Currency.Commands.DeleteData;

public class DeleteDataCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteDataCommandHandler : IRequestHandler<DeleteDataCommand>
{
    public ICurrencyConverter _currencyConverter { get; }
    public DeleteDataCommandHandler(ICurrencyConverter currencyConverter)
    {
        _currencyConverter = currencyConverter;
    }



    public async Task<Unit> Handle(DeleteDataCommand request, CancellationToken cancellationToken)
    {
        _currencyConverter.ClearConfiguration();

        return await Task.FromResult(Unit.Value);
    }
}
