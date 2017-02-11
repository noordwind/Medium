namespace Medium.Domain
{
    public enum Comparison
    {
        None = 0,
        Equals = 1,
        NotEqual = 2,
        StartsWith = 3,
        Contains = 4,
        GreaterThan = 5,
        LesserThan = 6,
        RegexMatch = 7
    }
}