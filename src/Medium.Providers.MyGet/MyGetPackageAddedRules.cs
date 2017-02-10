using Medium.Domain;

namespace Medium.Providers.MyGet
{
    public class MyGetPackageAddedRules : IRules
    {
        public string Provider { get; set; } = "myget";
        public Rule Branch { get; set; } 
        public Rule Tag { get; set; } 
        public Rule Version { get; set; }
        public bool MatchAll { get; set; }
    }
}