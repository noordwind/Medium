using System;
using Lockbox.Client;
using Medium.Integrations.AspNetCore.Configuration;
using Newtonsoft.Json;

namespace Medium.Integrations.Lockbox
{
    public class LockboxMediumSettingsLoader : IMediumSettingsLoader
    {
        private static readonly string EncryptionKeyEnvironmentVariable = "LOCKBOX_ENCRYPTION_KEY";
        private static readonly string ApiUrlEnvironmentVariable = "LOCKBOX_API_URL";
        private static readonly string ApiKeyEnvironmentVariable = "LOCKBOX_API_KEY";
        private static readonly string BoxNameEnvironmentVariable = "LOCKBOX_BOX_NAME";
        private static readonly string EntryKeyEnvironmentVariable = "LOCKBOX_ENTRY_KEY";
        private string _encryptionKey;
        private string _apiUrl;
        private string _apiKey;
        private string _boxName;
        private string _entryKey;

        public LockboxMediumSettingsLoader(string encryptionKey = null,  string apiUrl = null, 
            string apiKey = null, string boxName = null, string entryKey = null)
        {
        }

        public string Load()
        {
            _encryptionKey = GetParameterOrFail(_encryptionKey, EncryptionKeyEnvironmentVariable, "encryption key");
            _apiUrl = GetParameterOrFail(_apiUrl, ApiUrlEnvironmentVariable, "API key");
            _apiKey = GetParameterOrFail(_apiKey, ApiKeyEnvironmentVariable, "API url");
            _boxName = GetParameterOrFail(_boxName, BoxNameEnvironmentVariable, "box name");
            _entryKey = GetParameterOrFail(_entryKey, EntryKeyEnvironmentVariable, "entry key");

            var lockboxClient = new LockboxEntryClient(_encryptionKey, _apiUrl, _apiKey);
            var entry = lockboxClient.GetEntryAsync(_boxName, _entryKey).Result;
            if (entry == null)
            {
                throw new ArgumentException($"Lockbox entry has not been found for key: '{_entryKey}'.", nameof(_entryKey));
            }

            return JsonConvert.SerializeObject(entry);
        }

        private static string GetParameterOrFail(string parameter, string environmentVariable, string parameterName)
        {
            parameter = string.IsNullOrWhiteSpace(parameter) ? Environment.GetEnvironmentVariable(environmentVariable) : parameter;
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                return parameter;
            }

            throw new ArgumentException($"Lockbox {parameterName} can not be empty!", nameof(parameter));
        }
    }
}