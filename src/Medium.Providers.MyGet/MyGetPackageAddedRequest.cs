using System;
using Medium.Domain;

namespace Medium.Providers.MyGet
{
    public class MyGetPackageAddedRequest : IRequest
    {
        public Guid Identifier { get; set; }
        public string Username { get; set; }
        public DateTime When { get; set; }
        public MyGetPayload Payload { get; set; }

        public class MyGetPayload
        {
            public string PackageType { get; set; }
            public string PackageVersion { get; set; }
            public string FeedIdentifier { get; set; }
        }
    }
}