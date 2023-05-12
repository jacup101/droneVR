using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MotionControls : MonoBehaviour
{
    // Handles movement of player through motion controls
    // Container for old position vectors
    // Index 0 - left
    // Index 1 - right
    Vector3[] oldPos = { new Vector3 { x = 0, y = 0, z = 0 }, 
                         new Vector3 { x = 0, y = 0, z = 0} };
    // Limits, defined by definitions json
    public float[] limits = new float[4];
    // Speed of movement in both x and y direction (usually the same)
    float xspeed = 20f;
    float yspeed = 20f;
    // Threshold to ensure movement only occurs when both controllers are moved
    float threshhold = 0.001f;
    // Startup frame limit to prevent faulty controller position data, as well as containers for start and end frames
    public int frameLimit = 10;
    private int startFrame;
    private int endFrame;
    // State variables
    public bool dead = false;
    public bool paused = false;

    void Start()
    {
        Debug.Log("offbeat: Starting motion controls");
        OVRInput.Update();
        oldPos[0] = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        oldPos[1] = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        startFrame = Time.frameCount;
        endFrame = startFrame + frameLimit;
    }

    // Update is called once per frame
    void Update()
    {
        // If Game not paused, run motion controls
        if(!paused) {
            
            // Grab current positions of controllers
            Vector3 localLeftPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            Vector3 localRightPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            // Allow some startup time to prevent the object disappearing on Oculus
            // Debug.LogFormat("offbeat: {0} frame", Time.frameCount);
            if(startFrame <  endFrame) {
                oldPos[0] = localLeftPos;
                oldPos[1] = localRightPos;
                // Debug.LogFormat("x {0} y {1} z {2}", transform.position.x, transform.position.y, transform.position.z);
                startFrame ++;
            }

            // Get the delta for each controller
            Vector2 leftDelta = GetDifference(localLeftPos, oldPos[0]);
            Vector2 rightDelta = GetDifference(localRightPos, oldPos[1]);
                    
            // Provides compatibility for playing in Unity editor using keyboard controllers, conditionally compiled
            /*#if UNITY_EDITOR
            float yin = Input.GetAxis("Vertical");
            float xin = Input.GetAxis("Horizontal");
            leftDelta = new Vector2(xin, yin);
            rightDelta = new Vector2(xin, yin);
            xspeed = .2f;
            yspeed = .2f;
            #endif 
            */
            
            // Figure out what the motion actually translates to into an action
            String actionString = GetAction(rightDelta, leftDelta);

            // Chatty, disable for prod
            // Print the deltas
            // Debug.LogFormat("dronesaber: actions to take: {0}", actionString);  
            // Debug.LogFormat("dronesaber: positionDSTag x: {0} y: {1}", transform.position.x, transform.position.y);
            
            // As long as the player is not dead, handle movement
            if(!dead) {
                MoveDrone(leftDelta, rightDelta, actionString);
            }

            // Store the historical data for the controllers, to be used in next iteration
            oldPos[0] = localLeftPos;
            oldPos[1] = localRightPos;
        }
    }
    // Moves the drone based on an action
    void MoveDrone(Vector2 leftDelta, Vector2 rightDelta, String action) {
        Vector3 newPos = transform.position;
        // Move right or left as long as it does not pass the x limits
        if(action.Contains("right") && transform.position.x < limits[1]) {
            newPos.x += xspeed * Math.Abs((leftDelta.x + rightDelta.x) / 2);
        } else if(action.Contains("left") && transform.position.x > limits[0]) {
            newPos.x -= xspeed * Math.Abs((leftDelta.x + rightDelta.x) / 2);
        }
        // Move up or down as long as it does not pass the y limits
        if(action.Contains("up") && transform.position.y < limits[3]) {
            newPos.y += yspeed * Math.Abs((leftDelta.y + rightDelta.y) / 2);
        } else if(action.Contains("down") && transform.position.y > limits[2]) {
            newPos.y -= yspeed * Math.Abs((leftDelta.y + rightDelta.y) / 2);
        }
        // Set the new position as the objects position
        transform.position = newPos;
    }

    // Method to get the difference between two vectors, for both x and y. We ignore z for this game
    Vector2 GetDifference(Vector3 current, Vector3 old) {
        float deltaX = current.x - old.x;
        float deltaY = current.y - old.y;
        return NormalizeVector(new Vector2(deltaX, deltaY), threshhold);
    }
    // Method to "normalize" a movement vector
    // Sets small movements equal to 0, when below a defined threshold above
    Vector2 NormalizeVector(Vector2 current, float threshold) {
        if(Math.Abs(current.x) < threshold) {
            current.x = 0;
        }
        if(Math.Abs(current.y) < threshold) {
            current.y = 0;
        }
        return current;
    }
    // Examines the right and left controller movement and decides on an appropriate action to take
    // TODO: Add so that this method also takes in currently running tricks, so that it does not confuse itself
    // TODO: Add tricks
    // Note: Movement tricks are not yet implemented, only tricks using buttons are implemented. However, the TODOs above outline a future feature that can happen
    String GetAction(Vector2 right, Vector2 left) {
        String actionString = "";
        String rightC = SpecifyDir(right);
        String leftC = SpecifyDir(left);

        if(rightC.Contains("l") && leftC.Contains("l")) {
            actionString += "left";
        } if(rightC.Contains("r") && leftC.Contains("r")) {
            actionString += "right";
        } if(rightC.Contains("d") && leftC.Contains("d")) {
            actionString += "up";
        } if(rightC.Contains("u") && leftC.Contains("u")) {
            actionString += "down";
        } if(actionString.Equals("")) {
            actionString += "none";
        }
        return actionString;
    }
    // See which direction a controller has moved
    String SpecifyDir(Vector2 delta) {
        String dirString = "";
        if(delta.x == 0 && delta.y == 0) {
            dirString += "n";
        }
        if(delta.x < 0) {
            dirString += "l";
        } else if(delta.x > 0) {
            dirString += "r";
        }
        if(delta.y < 0) {
            dirString += "u";
        } else if(delta.y > 0) {
            dirString += "d";
        }

        return dirString;
    }
    // Method to tell the motion controllers to stop, when the player loses
    public void Die() {
        this.dead = true;
    }
    // Resets the motion controls in the event of a game restart
    public void Reset() {
        this.transform.position = new Vector3(0, 0, 5);
        startFrame = Time.frameCount;
        endFrame = startFrame + frameLimit;
        dead = false;
        paused = false;
    }
    // Pauses the motion controls
    public void Pause() {
        this.paused = true;
    }
    // Unpauses the motion controls
    public void Unpause() {
        startFrame = Time.frameCount;
        endFrame = startFrame + frameLimit;
        this.paused = false;
    }
}
