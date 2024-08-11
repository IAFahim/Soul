using System;
using System.Text;
using Alchemy.Inspector;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime.Reports
{
    public class ApiKeys : ScriptableObject
    {
        public bool logEnabled;

        [SerializeField] private string apiKeyBase64;
        [SerializeField] private string tokenBase64;

        public string ApiKey()
        {
            return ToKeyNotify(apiKeyBase64, "ApiKey");
        }

        public string Token()
        {
            return ToKeyNotify(tokenBase64, "Token");
        }

        [Button]
        private void ConvertToBase64(string apiKey, string token)
        {
            apiKeyBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(apiKey));
            tokenBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));
        }

        private string ToKeyNotify(string base64, string keyName, int peek = 3)
        {
            var key = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            if (logEnabled)
            {
                Debug.Log($"{keyName} = {key.Substring(0, peek) + "..." + key.Substring(key.Length - peek)}");
            }

            return key;
        }
    }
}