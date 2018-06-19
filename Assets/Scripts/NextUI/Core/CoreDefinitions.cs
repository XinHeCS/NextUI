using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreDefinitions : MonoBehaviour {

    // The possition type of UI element
    public enum UIFontType
    {
        // Normal window: 
        // Individual window which will not influence other UI elements
        Normal,
        // Fixed window:
        // Container window which will be the parent window of some other UI elements.
        // Usually as a root window.
        Fixed,
        // Pop window:
        // Used to display information
        PopUp
    }

    // The display mode od UI element
    public enum UIShowMode
    {
        // Normal display
        General,
        // Reserve change
        ReserveChange,
        // Hiden all
        // when in this mode, UI manager will hide all oter UI elements
        HideOther
    }

    // Transparency of the UI element
    public enum UILuencyType
    {        
        Lucency,
        Translucence,
        ImPenetrable,
        Pentrate
    }
}
