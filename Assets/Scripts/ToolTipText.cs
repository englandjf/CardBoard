using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipText : MonoBehaviour {

    [SerializeField] Text _ToolTip;


	// Use this for initialization
	void Start () {
        TileAssistant.ToolTip = this;
        _ToolTip.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 ScreenPt = Input.mousePosition;
        ScreenPt.z = 10;
        this.transform.position = Camera.main.ScreenToWorldPoint(ScreenPt);
	}

    public void ShowText(string text)
    {
        _ToolTip.text = text;
    }

    public void HideText()
    {
        _ToolTip.text = "";
    }
}
