using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextUI;

public class TestFramework : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIManager.GetInstance().ShowUIForm("LogonUIForm");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
