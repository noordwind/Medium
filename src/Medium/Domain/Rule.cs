namespace Medium.Domain
{
    public class Rule
    {
        public string Value { get; protected set; }
        public Comparison Comparison { get; protected set; }

        protected Rule()
        {
        }

        public Rule(string value, Comparison comparison)
        {
            Value = value;
            Comparison = comparison;
        }
    }
}