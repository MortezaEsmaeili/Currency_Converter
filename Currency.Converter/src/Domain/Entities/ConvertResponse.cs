namespace Currency.Converter.Domain.Entities;

public class ConvertResponse
{
    public string _resultDescription { get; set; }
    public double _convertResult { get; set; }

    public ConvertResponse(string resultDescription, double convertResult)
    {
        this._resultDescription = resultDescription;
        this._convertResult = convertResult;
    }

    
}
