using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextUI
{
    public class JsonConfigManager : IConfigManager
    {
        private Dictionary<string, string> _appSettings;

        public Dictionary<string, string> AppSettings
        {
            get
            {
                return _appSettings;
            }
        }

        public JsonConfigManager(string configPath)
        {
            _appSettings = new Dictionary<string, string>();
            LoadConfigFile(configPath);
        }
        
        public int GetSettingNumber()
        {
            if (_appSettings != null)
            {
                return _appSettings.Count;
            }

            return 0;
        }

        // Read the config file
        private void LoadConfigFile(string configPath)
        {
            KeyValueInfo configObj;

            if (string.IsNullOrEmpty(configPath))
            {
                Debug.LogError("Invalid file directory!");
                return;
            }

            try
            {
                TextAsset configTxt = Resources.Load<TextAsset>(configPath);
                configObj = JsonUtility.FromJson<KeyValueInfo>(configTxt.text);
            }
            catch
            {
                throw new JsonAnalasysException(GetType() + "Initalize JsonConfigManager " +
                    "parameter: " + configPath);
            }

            // Add all cinfig info into appsettings
            foreach (KeyValueNode node in configObj.ConfigInfo)
            {
                _appSettings.Add(node.Key, node.Value);
            }
        }
    }
}
