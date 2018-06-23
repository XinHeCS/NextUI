using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextUI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _currentManager = null;

        // Store all the UI prefabs and their paths
        // Key: name of the prefab
        // Value: related path of the prefab
        private Dictionary<string, string> _prefabsDic;

        // Store all the UI forms which have been laoded so far
        // Key: name of the prefab
        // Value: related path of the prefab
        private Dictionary<string, BaseUIForm> _allFormsCach;

        // All the forms which are shown currently
        private Dictionary<string, BaseUIForm> _showFormsCach;

        // Root of current canvas
        private Transform _currentRootCanvas = null;

        // Root of all normal forms
        private Transform _currentNormal;
        // Root of all FIxed forms
        private Transform _currentFixed;
        // Root of all popup forms
        private Transform _currentPopUp;
        // All UI scripts manager
        private Transform _UIScripts;

       // ===================UI Manager public methods================
       public static UIManager GetInstance()
        {
            if (!_currentManager)
            {
                _currentManager = new GameObject("UIManager").AddComponent<UIManager>();
            }

            return _currentManager;
        }

        public void Awake()
        {
            // Initial all fields
            _prefabsDic = new Dictionary<string, string>();
            _allFormsCach = new Dictionary<string, BaseUIForm>();
            _showFormsCach = new Dictionary<string, BaseUIForm>();

            if (!InitRootCanvas())
            {
                Debug.LogError("Initialize error!!");
                Destroy(this);

                return;
            }

            // Add self as a child of UISCripts nodes
            gameObject.transform.SetParent(_UIScripts, false);
            // UIManager will not destroyed when changing the schene
            DontDestroyOnLoad(_currentRootCanvas);

            // Set the default UI prefabs paths
            if (_prefabsDic != null)
            {
// To be improved ...
                _prefabsDic.Add("LogIn", @"TestForm\LogIn");
// To be improved ...
            }
        }

        public void ShowUIForm(string uiName)
        {
            BaseUIForm uiForm = LaodUIFormCach(uiName);

            if (uiForm == null)
            {
                Debug.LogError("Laoding error!");
                return;
            }

            // Display the ui form according to its show mode
            switch (uiForm.CurrrentUIType.showMode)
            {
                case UIShowMode.General:
                    AddToCurrentShow(uiName);
                    break;
                case UIShowMode.ReserveChange:
                    // TODO
                    break;
                case UIShowMode.HideOther:
                    // TODO
                    break;
            }
        }


        // ===================UI Manager private methods================
        private bool InitRootCanvas()
        {
// To be improved ...
            var rootCanvas = Resources.Load("NextUI") as GameObject;
            var canvasClone = Instantiate<GameObject>(rootCanvas, Vector3.zero, Quaternion.identity);
// To be improved ...

            if (!rootCanvas)
            {
                Debug.LogError("Failed to load resources: NextUI");

                return false;
            }

            _currentRootCanvas = canvasClone.transform;
            _currentNormal = _currentRootCanvas.Find("Normal");
            _currentFixed = _currentRootCanvas.Find("Fixed");
            _currentPopUp = _currentRootCanvas.Find("PopUp");
            _UIScripts = _currentRootCanvas.Find("UIScripts");

            return true;
        }

        // Load a UI form to the cach according to it's name
        private BaseUIForm LaodUIFormCach(string uiName)
        {
            var uiForm = QueryUIForm(uiName);
            if (uiForm == null)
            {
                return ReadUIForm(uiName);
            }

            return uiForm;
        }

        // Read the UI prefab from certain directory
        private BaseUIForm ReadUIForm(string uiName)
        {
            // Find the path according to uiName
            string path = QueryUIFormPath(uiName);
            GameObject cloneUI = null;
            if (!string.IsNullOrEmpty(path))
            {
// To be improved ...
                var uiObj = Resources.Load(path) as GameObject;
                cloneUI = Instantiate<GameObject>(uiObj, Vector3.zero, Quaternion.identity);
// To be improved ...
            }

            // Initalize the cloned prefab according to it's info
            if (cloneUI != null)
            {
                BaseUIForm baseForm = cloneUI.AddComponent<BaseUIForm>();
                if (baseForm == null)
                {
                    Debug.LogError("Config file error!!");
                    Debug.LogError("Not a NextUI element.");

                    return null;
                }

                // Set the parent of current UIForm
                switch (baseForm.CurrrentUIType.formType)
                {
                    case UIFormType.Normal:
                        cloneUI.transform.SetParent(_currentNormal, false);
                        break;
                    case UIFormType.Fixed:
                        cloneUI.transform.SetParent(_currentFixed, false);
                        break;
                    case UIFormType.PopUp:
                        cloneUI.transform.SetParent(_currentPopUp, false);
                        break;
                }

                // Hide the ui form temparary
                baseForm.Hide();

                // Add current UIForm into all forms cach
                _allFormsCach.Add(uiName, baseForm);

                return baseForm;
            }
            else
            {
                Debug.LogError("Prefab: " + uiName + " not found!!");

                return null;
            }

        }

        // Add the ui form into the queue of current shown forms
        private void AddToCurrentShow(string uiName)
        {
            BaseUIForm uiForm = QueryUIForm(uiName);

            _showFormsCach.Add(uiName, uiForm);
            uiForm.Display();
        }
        
        // Get the paths of queried ui form
        private string QueryUIFormPath(string uiName)
        {
            string uiPath;
            _prefabsDic.TryGetValue(uiName, out uiPath);

            return uiPath;
        }

        // Return the UIForm according to its name
        private BaseUIForm QueryUIForm(string uiName)
        {
            BaseUIForm queryResult;
            _allFormsCach.TryGetValue(uiName, out queryResult);

            return queryResult;
        }

        // Query the laoding status of current ui form
        private bool IsLoaded(string uiName)
        {
            return _allFormsCach.ContainsKey(uiName);            
        }

        // Query the displaying status of current ui form
        private bool IsShown(string uiName)
        {
            return _showFormsCach.ContainsKey(uiName);
        }
    }
}
