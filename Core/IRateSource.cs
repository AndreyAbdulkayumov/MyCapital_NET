namespace Core;

public enum TypeOfCurrency
{
    NotDefined,
    Ruble,
    Dollar,
    Euro
}

public struct Rate
{
    public TypeOfCurrency Currency;
    public double Value;
}

public interface IRateSource
{
    string UpdateDate { get; }
    string SourceName { get; }
    string SourceAddress { get; }
    void Init();
    Rate GetRate(TypeOfCurrency type);
}
