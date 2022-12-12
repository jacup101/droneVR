using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandTrackingUI : MonoBehaviour
{
    // Helper class to handle ray tracing for main menu UI
    public OVRHand hand;
    public OVRInputModule inputModule;
    void Start()
    {
        inputModule.rayTransform = hand.PointerPose;
    }
}
