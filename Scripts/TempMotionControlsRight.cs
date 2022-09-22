using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TempMotionControlsRight : MonoBehaviour
{
    Vector3 oldPos = new Vector3(0f, 0f, 0f);
    

    // Start is called before the first frame update
    
    void Start()
    {
       //Debug.Log("dronesaber: Hello World");
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        Vector3 localLeftPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        //Debug.Log("dronesaber: " + localLeftPos);

        float speed = 10;

        float deltaX = localLeftPos.x - oldPos.x;
        float deltaY = localLeftPos.y - oldPos.y;

        //transform.position = new Vector3(transform.position.x + speed * deltaX, transform.position.y + speed * deltaY, transform.position.z);

        oldPos = localLeftPos;
    }
}
