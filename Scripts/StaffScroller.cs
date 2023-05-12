using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StaffScroller : MonoBehaviour
{

    Vector3 oldPos = new Vector2(0,0);
    int startFrame = 0;
    int endFrame = 20;

    float movementAmount = 15f;

    [SerializeField] 
    private GameObject noteStaff;
    [SerializeField] 
    private GameObject editorStaff;
    float threshhold = 0.005f;

    void Update() {
        OVRInput.Update();

        Vector3 localLeftPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        Vector3 localRightPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        // Allow some startup time to prevent the object disappearing on Oculus
        // Debug.LogFormat("offbeat: {0} frame", Time.frameCount);
        if(startFrame <  endFrame) {
            oldPos = localLeftPos;
            
            
            startFrame ++;
        }


        // Get the delta for each controller
        Vector2 leftDelta = GetDifference(localLeftPos, oldPos);
        bool indexLTriggerPressed = false;
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) > 0.2f)
          {
              indexLTriggerPressed = true;
          }

        if(indexLTriggerPressed) {

        }

        if (leftDelta[0] != 0f && indexLTriggerPressed) {
            float toMove = movementAmount * leftDelta[0];
            noteStaff.GetComponent<Staff>().MoveStaffX(toMove);
            editorStaff.GetComponent<Staff>().MoveStaffX(toMove);
        }

        // Store the historical data for the controllers, to be used in next iteration
        oldPos = localLeftPos;
    }

    Vector2 GetDifference(Vector3 current, Vector3 old) {
        float deltaX = current.x - old.x;
        float deltaY = current.y - old.y;
        return NormalizeVector(new Vector2(deltaX, deltaY), threshhold);
    }

    Vector2 NormalizeVector(Vector2 current, float threshold) {
        if(Math.Abs(current.x) < threshold) {
            current.x = 0;
        }
        if(Math.Abs(current.y) < threshold) {
            current.y = 0;
        }
        return current;
    }
}
