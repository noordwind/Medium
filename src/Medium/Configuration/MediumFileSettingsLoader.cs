using System.IO;

namespace Medium.Configuration
{
    public class MediumFileSettingsLoader : IMediumSettingsLoader
    {
        private readonly string _path;

        public MediumFileSettingsLoader(string path)
        {
            _path = path;
        }

        public string Load() => File.ReadAllText(_path);
    }
}