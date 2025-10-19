namespace CSharpEfAndAspNet;

public sealed class Error
{
    public string Message
    {
        get;
        set
        {
            if (field != null &&
                field != value)
            {
                field = value;
            }
        }
    }
}
