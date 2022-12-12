using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInTime : MonoBehaviour {
    // Moves each note in time with the music, so that it reaches the player's position when the note is played in the music
    // Movement floats
    public float seconds;
    public float timer;
    public float percent;
    public float speed;
    public float movementSpeed = .1f;
    // Position vectors
    public Vector3 start_point;
    public Vector3 end_point;
    public Vector3 diff;
    // State variables
    public bool moving;
    public bool transformed;
    public bool paused = false;
    public bool spawnAnim = false;
    // Animation variables
    public float rotationSpeed = 1f;
    public float totRot = 0f;
    public string animType;
    // List of game objects to be set by the rhythm system
    public List<GameObject> allObjects = null;
    // Horizontal movement variables
    public int modifier = 1;
    public List<int> limits;
    // Start is called before the first frame update
    void Start()
    {
        start_point = transform.position;
        diff = end_point - start_point;
    }
    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if(moving && !paused) {
            // Move the object towards its end point
            if(timer <= seconds) {
                timer += Time.deltaTime;
                percent = timer / seconds;

                transform.position = start_point + diff * percent;
                // Handle animation, if the object has some
                Animate();
            } else {
                if(!transformed) {
                    // Transform();
                    // Curently does nothing, but can be used in the future
                }
                transform.position = transform.position + new Vector3(0, 0, -1 * speed);
                // Destroy condition
                if (transform.position.z < 0) {
                    allObjects.Remove(gameObject);
                    Destroy(gameObject);
                }
            }
        }
        if(!spawnAnim) {
            // For future use, to add an animation upon spawning the note
            // (for example, flying up from the bottom, or something like that)
            int rand = Random.Range(0, 4);
            /*float xSpeed = rotationSpeed;
            float zSpeed = rotationSpeed;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + xSpeed, 0,  transform.rotation.eulerAngles.z + zSpeed);
            totRot += rotationSpeed;
            if(totRot > 180) {
                spawnAnim = true;
            } */
        }
    }
    // Handles animations for notes that have them
    void Animate() {
        float xSpeed = rotationSpeed;
        float zSpeed = rotationSpeed;
        // X is left right, Z is front back
        // Rotate around object's origin
        if(animType == "rotateX") {
            transform.Rotate(new Vector3(xSpeed, 0, 0), Space.Self);
        } if(animType == "rotateZ") {
            transform.Rotate(new Vector3(0, 0, zSpeed), Space.Self);
        } if(animType == "rotateXZ") {
            transform.Rotate(new Vector3(xSpeed, 0, zSpeed), Space.Self);
        }
        // Handles notes that move left and right, while also moving forwards
        if (animType == "move_right_left") {
            start_point = new Vector3(start_point.x + (movementSpeed * modifier), start_point.y, start_point.z);
            if (transform.position.x >= limits[1]) {
                modifier = -1;
            }
            if (transform.position.x <= limits[0]) {
                modifier = 1;
            }
        }
    }
    // Not currently used
    // Changes the color of the object when it hits the location of the player
    // TODO: Think of another way to represent this which is not color
    public void Transform() {
        try {
            GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        } catch {
            // Debug.Log("object does not have a renderer, is it complex?");
        }
        transformed = true;
    }
    // Sets up all variables required for the ntoe to move in time properly, called upon creation of a note
    public void Setup(float seconds, Vector3 end_point, string anim, List<GameObject> allObj, List<int> limits) {
        this.seconds = seconds;
        this.start_point = transform.position;
        this.end_point = end_point;
        this.diff = this.end_point - this.start_point;
        this.moving = true;
        this.allObjects = allObj;
        this.animType = anim;
        this.limits = limits;
    }
    // Pauses movement
    public void Pause() {
        this.paused = true;
    }
    // Unpauses movement
    public void Unpause() {
        this.paused = false;
    }
}
