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

        // Store the module UIForms
        private Stack<BaseUIForm> _moduelFormsStack;

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
            _moduelFormsStack = new Stack<BaseUIForm>();

            if (!InitRootCanvas())
            {
                Debug.LogError("Initialize error!!");
                Destroy(this);
            }

            // Add self as a child of UISCripts nodes
            gameObject.transform.SetParent(_UIScripts, false);
            // UIManager will not destroyed when changing the schene
            DontDestroyOnLoad(_currentRootCanvas);

            // Set the default UI prefabs paths
            if (_prefabsDic != null)
            {
// To be improved ...
                _prefabsDic.Add("LogIn", @"UIPrefabs\LogonUIForm");
// To be improved ...
            }
        }

        public void ShowUIForm(string uiName)
        {
            BaseUIForm uiForm = LoadUIFormCach(uiName);

            if (uiForm == null)
            {
                Debug.LogError("Laoding error!");
                return;
            }

            if (uiForm.CurrentUIType.isClearReverseChange)
            {
                ClearModuelStack();
            }

            // Display the ui form according to its show mode
            switch (uiForm.CurrentUIType.showMode)
            {
                case UIShowMode.General:
                    AddToCurrentShow(uiName, uiForm);
                    break;
                case UIShowMode.ReverseChange:
                    AddToModuelStack(uiName, uiForm);
                    break;
                case UIShowMode.HideOther:
                    AddToHideOthers(uiName, uiForm);
                    break;
            }
        }

        public void CloseUIForm(string uiName)
        {
            if (!IsLoaded(uiName))
            {
                Debug.LogWarning("Moduel: " + uiName + " unloaded!");
                return;
            }

            var uiForm = LoadUIFormCach(uiName);

            // Close the ui form according to it's UIType
            switch (uiForm.CurrentUIType.showMode)
            {
                case UIShowMode.General:
                    RemoveFromCurrentShow(uiName, uiForm);
                    break;
                case UIShowMode.ReverseChange:
                    RemoveFromModuelStack(uiName, uiForm);
                    break;
                case UIShowMode.HideOther:
                    RemoveFromHideOthers(uiName, uiForm);
                    break;
            }
        }


        // ===================UI Manager private methods================
        private bool InitRootCanvas()
        {
// To be improved ...
            var rootCanvas = Resources.Load(GlobalConfig.CONFIG_CANVAS) as GameObject;
            var canvasClone = Instantiate<GameObject>(rootCanvas, Vector3.zero, Quaternion.identity);
// To be improved ...

            if (!rootCanvas)
            {
                Debug.LogError("Failed to load resources: NextUI");

                return false;
            }

            _currentRootCanvas = canvasClone.transform;
            _currentNormal = _currentRootCanvas.Find(GlobalConfig.CONFIG_NORMAL);
            _currentFixed = _currentRootCanvas.Find(GlobalConfig.CONFIG_FIXED);
            _currentPopUp = _currentRootCanvas.Find(GlobalConfig.CONFIG_POPUP);
            _UIScripts = _currentRootCanvas.Find(GlobalConfig.CONFIG_SCRIPT);

            return true;
        }

        // Load a UI form to the cach according to it's name
        private BaseUIForm LoadUIFormCach(string uiName)
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
                BaseUIForm baseForm = cloneUI.GetComponent<BaseUIForm>();
                if (baseForm == null)
                {
                    Debug.LogError("Config file error!!");
                    Debug.LogError("Not a NextUI element.");

                    return null;
                }

                // Set the parent of current UIForm
                switch (baseForm.CurrentUIType.formType)
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
        private void AddToCurrentShow(string uiName, BaseUIForm uiForm)
        {
            _showFormsCach.Add(uiName, uiForm);
            uiForm.Display();
        }

        // Add the moduel ui form into the moduel stack
        private void AddToModuelStack(string uiName, BaseUIForm uIForm)
        {
            // Check if there exist ui forms in the stack
            // and freeze the previous form
            if (_moduelFormsStack.Count > 0)
            {
                BaseUIForm preForm = _moduelFormsStack.Peek();
                preForm.Freeze();
            }

            _moduelFormsStack.Push(uIForm);
            uIForm.Display();
        }

        // Add the hide other forms into the qeueu of current show forms
        private void AddToHideOthers(string uiName, BaseUIForm uiForm)
        {
            // Hide all the other forms
            foreach (BaseUIForm item in _allFormsCach.Values)
            {
                item.Hide();
            }
            foreach (BaseUIForm item in _moduelFormsStack)
            {
                item.Hide();
            }

            // Add current form into the queue of show forms
            _showFormsCach.Add(uiName, uiForm);
            uiForm.Display();
        }

        // Remove the ui form from the queue of current shown forms
        private void RemoveFromCurrentShow(string uiName, BaseUIForm uiForm)
        {
            uiForm.Hide();
            _showFormsCach.Remove(uiName);
        }

        // Remove the moduel ui form from the moduel stack
        private void RemoveFromModuelStack(string uiName, BaseUIForm uiForm)
        {
            // Only allowed to close the top form of the stack
            if (_moduelFormsStack.Peek() != uiForm)
            {
                return;
            }
            uiForm.Hide();
            _moduelFormsStack.Pop();
            if (_moduelFormsStack.Count > 0)
            {
                var preForm = _moduelFormsStack.Peek();
                preForm.Redisplay();
            }
        }

        // Remove the hide other forms from the qeueu of current show forms
        private void RemoveFromHideOthers(string uiName, BaseUIForm uiForm)
        {
            uiForm.Hide();
            _showFormsCach.Remove(uiName);

            // Redisplay other forms
            foreach (BaseUIForm item in _showFormsCach.Values)
            {
                item.Redisplay();
            }
            foreach (BaseUIForm item in _moduelFormsStack)
            {
                item.Redisplay();
            }
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

        // Clear the moduel stack
        private bool ClearModuelStack()
        {
            if (_moduelFormsStack != null && _moduelFormsStack.Count >= 1)
            {
                _moduelFormsStack.Clear();

                return true;
            }

            return false;
        }
    }
}
