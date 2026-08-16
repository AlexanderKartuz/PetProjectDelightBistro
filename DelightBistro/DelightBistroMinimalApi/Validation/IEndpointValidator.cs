namespace DelightBistroMinimalApi.Validation
{
    public interface IEndpointValidator
    {
        IResult? Validate(object model);
    }
}