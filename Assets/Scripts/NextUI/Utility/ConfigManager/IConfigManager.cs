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


[SerializeField]
internal class KeyValueNode
{
    public string Key = null;
    public string Value = null;
}

[SerializeField]
internal class KeyValueInfo
{
    public List<KeyValueNode> ConfigInfo = null;
}
