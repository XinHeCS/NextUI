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
        private Dictionary<string, BaseUIForm> _allFormsDic;

        // All the forms which are shown currently
        private Dictionary<string, BaseUIForm> _showFormsDic;

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
            _allFormsDic = new Dictionary<string, BaseUIForm>();
            _showFormsDic = new Dictionary<string, BaseUIForm>();

            if (!InitRootCanvas())
            {
                Debug.LogError("Initialize error!!");
                Destroy(_currentManager);

                return;
            }

            // Add self as a child of UISCripts nodes
            gameObject.transform.SetParent(_UIScripts, false);
            // UIManager will not destroyed when changing the schene
            DontDestroyOnLoad(_currentRootCanvas);


        }


        // ===================UI Manager private methods================
        private bool InitRootCanvas()
        {
            var rootCanvas = Resources.Load("NextUI") as GameObject;

            if (!rootCanvas)
            {
                Debug.LogError("Failed to load resources: NextUI");

                return false;
            }

            _currentRootCanvas = rootCanvas.transform;
            _currentNormal = _currentRootCanvas.Find("Normal");
            _currentFixed = _currentRootCanvas.Find("Fixed");
            _currentPopUp = _currentRootCanvas.Find("PopUp");
            _UIScripts = _currentRootCanvas.Find("UIScripts");

            return true;
        }
    }
}
