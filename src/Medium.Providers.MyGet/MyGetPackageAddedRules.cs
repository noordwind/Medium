using Medium.Domain;

namespace Medium.Providers.MyGet
{
    public class MyGetPackageAddedRules
    {
        public Rule PackageIdentifier { get; set; }
        public Rule Branch { get; set; } 
        public Rule Tag { get; set; } 
        public Rule Version { get; set; }
        public bool MatchAll { get; set; }
    }
}