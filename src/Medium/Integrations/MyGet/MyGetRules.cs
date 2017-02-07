using Medium.Domain;

namespace Medium.Integrations.MyGet
{
    public class MyGetRules : IRules
    {
        public Rule Branch { get; set; } 
        public Rule Tag { get; set; } 
        public Rule Version { get; set; }
        public bool MatchAll { get; set; }
    }
}