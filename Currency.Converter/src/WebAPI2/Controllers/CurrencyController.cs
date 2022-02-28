using Currency.Converter.Application.Currency.Commands.DeleteData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Currency.Converter.Domain.Entities;
using Currency.Converter.Application.Currency.Commands.UpdateConfig;
using Currency.Converter.Application.Currency.Query.Convert;

namespace WebAPI2.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("ClearConfiguration")]
    public async Task<ActionResult> ClearConfiguration()
    {
        await _mediator.Send(new DeleteDataCommand());
        return Ok();
    }

    [HttpPost]
    [Route("UpdateConfiguration")]
    public async Task<ActionResult> UpdateConfiguration([FromBody] CurrencyRate[] currencyRates)
    {
        UpdateConfigCommand updateConfigCommand = new(currencyRates);
        await _mediator.Send(updateConfigCommand);
        return Ok();
    }

    [HttpGet]
    [Route("Convert")]
    public async Task<ActionResult> Convert(string fromCurrency, string toCurrency, double amount)
    {
        ConvertCurrencyQuery currencyQuery = new(fromCurrency,toCurrency,amount);
        var response = await _mediator.Send(currencyQuery);
        return Ok(response);
    }
}
