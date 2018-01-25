using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MapObject
{
    Vector3 _prevPosition = Vector3.zero;

    // Unity Functions

    void Start ()
    {
        _prevPosition = transform.position;
    }
	
	void Update ()
    {
        if(Input.GetMouseButton(2))
        {
            /*
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);            
            float distance = Vector3.Distance(curPosition, gameObject.transform.position);
            if(5.0f < distance)
            {
                Vector3 deltaPosition = _prevPosition - curPosition;
                Debug.Log("delta " + deldistancetaPosition);
                gameObject.transform.position += deltaPosition;                
            }
            _prevPosition = transform.position;
            */
        }
    }
}
