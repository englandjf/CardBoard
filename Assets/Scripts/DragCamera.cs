using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour
{

    [SerializeField] private Camera _CurrentCamera;

    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public bool cameraDragging = true;

    private int _BLX, _BLY, _TRX, _TRY;
    
    public void CreateCameraBarrierValues(int BLx, int BLy, int TRx, int TRy)
    {
        _BLX = BLx;
        _BLY = BLy;
        _TRX = TRx;
        _TRY = TRy;
    }

    void Update()
    {



        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        float left = Screen.width * 0.2f;
        float right = Screen.width - (Screen.width * 0.2f);
        float top = Screen.height * 0.2f;
        float bottom = Screen.height - (Screen.height * 0.2f);

        if (mousePosition.x < left)
        {
            cameraDragging = true;
        }
        else if (mousePosition.x > right)
        {
            cameraDragging = true;
        }
        else if (mousePosition.y < top)
        {
            cameraDragging = true;
        }
        else if(mousePosition.y > bottom)
        {
            cameraDragging = true;
        }


        

        if (cameraDragging)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1)) return;
#elif UNITY_ANDROID
            if (Input.touchCount  == 2)
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (Input.touchCount != 2) return;
#endif

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed) * -1;

            if (_CurrentCamera.transform.position.x < _TRX && _CurrentCamera.transform.position.x > _BLX
                && _CurrentCamera.transform.position.z < _TRY && _CurrentCamera.transform.position.z > _BLY)
            {
                _CurrentCamera.transform.Translate(move, Space.World);
            }
            else if(_CurrentCamera.transform.position.x >= _TRX && move.x < 0f)
            {
                _CurrentCamera.transform.Translate(move, Space.World);
            }
            else if(_CurrentCamera.transform.position.x <= _BLX && move.x > 0f)
            {
                _CurrentCamera.transform.Translate(move, Space.World);
            }
            else if(_CurrentCamera.transform.position.z >= _TRY && move.z < 0f)
            {
                _CurrentCamera.transform.Translate(move, Space.World);
            }
            else if(_CurrentCamera.transform.position.z <= _BLY && move.z > 0f)
            {
                _CurrentCamera.transform.Translate(move, Space.World);
            }
        }
    }


}