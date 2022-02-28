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
    private readonly ILogger<CurrencyController> _logger;

    public CurrencyController(IMediator mediator, ILogger<CurrencyController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Route("ClearConfiguration")]
    public async Task<ActionResult> ClearConfiguration()
    {
        _logger.LogInformation("ClearConfiguration API Method was called");
        await _mediator.Send(new DeleteDataCommand());
        return Ok();
    }

    [HttpPost]
    [Route("UpdateConfiguration")]
    public async Task<ActionResult> UpdateConfiguration([FromBody] CurrencyRate[] currencyRates)
    {
        _logger.LogInformation("UpdateConfiguration API Method was called");
        UpdateConfigCommand updateConfigCommand = new(currencyRates);
        await _mediator.Send(updateConfigCommand);
        return Ok();
    }

    [HttpGet]
    [Route("Convert")]
    public async Task<ActionResult> Convert(string fromCurrency, string toCurrency, double amount)
    {
        _logger.LogInformation("Convert API Method was called");
        ConvertCurrencyQuery currencyQuery = new(fromCurrency,toCurrency,amount);
        var response = await _mediator.Send(currencyQuery);
        return Ok(response);
    }
}
