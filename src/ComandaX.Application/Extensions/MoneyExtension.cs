namespace ComandaX.Application.Extensions;

public static class MoneyExtension
{
    public static decimal AsMoney(this decimal value)
    {
        return Math.Round(value, 2);
    }
}
