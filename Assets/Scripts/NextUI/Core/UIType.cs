using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextUI
{
    // Internal class to descirbe the core 
    // property of some UI node
    internal class UIType
    {
        public bool isClearReverseChange = false;
        public UIFormType formType = UIFormType.Normal;
        public UIShowMode showMode = UIShowMode.General;
        public UILuencyType luencyType = UILuencyType.Luency;
    }
}
