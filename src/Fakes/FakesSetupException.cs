namespace DerAlbert.Extensions.Fakes;

[Serializable]
public class FakesSetupException : Exception
{
    public FakesSetupException(string message) : base(message)
    {
    }

    public FakesSetupException(string message, Exception innerException) : base(message, innerException)
    {
    }
}