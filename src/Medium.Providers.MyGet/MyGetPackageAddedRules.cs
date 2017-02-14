using Medium.Domain;

namespace Medium.Providers.MyGet
{
    public class MyGetPackageAddedRules
    {
        public Rule PackageIdentifier { get; set; }
        public Rule FeedIdentifier { get; set; }
    }
}