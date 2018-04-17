using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Camera _CurrentCamera;
    private Rect _CameraRestrict;
    private Vector3 _LastValidPt = new Vector3();

	// Use this for initialization
	void Start () {
        _CurrentCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        bool ValidMove = _CameraRestrict.Contains(new Vector2(_CurrentCamera.transform.position.x, _CurrentCamera.transform.position.z),true);
        if(ValidMove)
        {
            Vector3 AdjPos = new Vector3();
            if(Input.mousePosition.x < 0)
            {
                AdjPos.x = -.1f;
            }
            else if (Input.mousePosition.x > Screen.width)
            {
                AdjPos.x += .1f;
            }
            if(Input.mousePosition.y < 0)
            {
                AdjPos.z += -.1f;
            }
            else if(Input.mousePosition.y > Screen.height)
            {
                AdjPos.z += .1f;
            }
            _LastValidPt = _CurrentCamera.transform.position;
            _CurrentCamera.transform.position += AdjPos;
           
        }
        else
        {
            _CurrentCamera.transform.position = _LastValidPt;
        }
    }


    public void SetCamera(Camera c)
    {
        _CurrentCamera = c;
    }

    public void CreateCameraBarrier(int BLx,int BLy,int TRx,int TRy)
    {
        _CameraRestrict = new Rect(BLx, BLy, TRx - BLx, TRy - BLy);
    }

    public void SetCameraPosition(Vector3 pos)
    {
        _CurrentCamera.transform.position = pos;
    }
}
