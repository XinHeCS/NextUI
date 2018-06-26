using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NextUI
{
    public class UIMask : MonoBehaviour
    {
        // Singleton
        private static UIMask _currentMask = null;

        // Global canvas node
        private Transform _rootCanvas;

        // Popup form group
        private Transform _currentPopUp;

        // UIScripts manager node
        private Transform _UIScripts;

        // Current Mask plane
        private GameObject _currentMaskPanel;

        // Access to singleton
        public static UIMask GetInstance()
        {
            if (_currentMask == null)
            {
                _currentMask = new GameObject("_UIMask").AddComponent<UIMask>();
            }

            return _currentMask;
        }


        public void Awake()
        { 
            if (!InitRootanvas())
            {
                Debug.LogError("Initialization Error!");
                Destroy(this);
            }

            // Initialize Mask pane
            _currentMaskPanel = _currentPopUp.Find(GlobalConfig.UTILITY_UIMASK).gameObject;
            if (!_currentMaskPanel)
            {
                Debug.LogError("UIMask load failed!");
                Destroy(this);
            }
        }

        private bool InitRootanvas()
        {
            // Get root canvas
            _rootCanvas = GameObject.Find(GlobalConfig.CONFIG_CANVAS).transform;
            if (!_rootCanvas)
            {
                Debug.LogError("Didn't load Canvas!");
                return false;
            }

            _UIScripts = _rootCanvas.Find(GlobalConfig.CONFIG_SCRIPT);
            _currentPopUp = _rootCanvas.Find(GlobalConfig.CONFIG_POPUP);
            if (!_UIScripts || !_currentPopUp)
            {
                Debug.LogError("Not standard main canvas!");
                return false;
            }
            gameObject.transform.SetParent(_UIScripts, false); 

            return true;
        }

        /// <summary>
        /// When a pop up form is displayed in the schene, 
        /// Use this function to set UIMask properly.
        /// </summary>
        /// <param name="uIForm">The pop up ui form</param>
        public void SetMask(BaseUIForm uIForm)
        {
            // BLock all the other operations except 
            // the new ui form
            _currentMaskPanel.transform.SetAsLastSibling();

            // Set the status of ui mask
            switch (uIForm.CurrentUIType.luencyType)
            {
                case UILuencyType.Luency:
                    _currentMaskPanel.GetComponent<Image>().color = 
                        new Color(
                            GlobalConfig.COLOR_LUENCY_RGB,
                            GlobalConfig.COLOR_LUENCY_RGB,
                            GlobalConfig.COLOR_LUENCY_RGB, 
                            GlobalConfig.COLOR_LUENCY_RGB_A);
                    _currentMaskPanel.SetActive(true);
                    break;
                case UILuencyType.Transluency:
                    _currentMaskPanel.GetComponent<Image>().color = 
                        new Color(
                            GlobalConfig.COLOR_TRANS_LUENCY_RGB,
                            GlobalConfig.COLOR_TRANS_LUENCY_RGB,
                            GlobalConfig.COLOR_TRANS_LUENCY_RGB,
                            GlobalConfig.COLOR_TRANS_LUENCY_RGB_A);
                    _currentMaskPanel.SetActive(true);
                    break;
                case UILuencyType.ImPenetrable:
                    _currentMaskPanel.GetComponent<Image>().color =
                        new Color(
                            GlobalConfig.COLOR_IMPENETRABLE_COLOR_RGB,
                            GlobalConfig.COLOR_IMPENETRABLE_COLOR_RGB,
                            GlobalConfig.COLOR_IMPENETRABLE_COLOR_RGB,
                            GlobalConfig.COLOR_IMPENETRABLE_COLOR_RGB_A);
                    _currentMaskPanel.SetActive(true);
                    break;
                case UILuencyType.Pentrate:
                    _currentMaskPanel.SetActive(false);
                    break;
            }
        }

        public void CancelMask()
        {
            _currentMaskPanel.SetActive(false);
        }
    }
}
