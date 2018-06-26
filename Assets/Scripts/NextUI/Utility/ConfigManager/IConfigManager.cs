using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextUI
{
    public interface IConfigManager
    {
        Dictionary<string, string> AppSettings
        {
            get;
        }

        int GetSettingNumber();
    }
}


[System.Serializable]
internal class KeyValueNode
{
    public string Key = null;
    public string Value = null;
}

[System.Serializable]
internal class KeyValueInfo
{
    public List<KeyValueNode> ConfigInfo = null;
}
