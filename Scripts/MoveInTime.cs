using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInTime : MonoBehaviour {

    public float seconds;
    public float timer;
    public float percent;
    public float speed;

    public Vector3 start_point;
    public Vector3 end_point;
    public Vector3 diff;
    public bool moving;
    public bool transformed;
    public bool spawnAnim = false;
    public float rotationSpeed = 5f;
    public float totRot = 0f;


    // Start is called before the first frame update
    void Start()
    {
        start_point = transform.position;
        diff = end_point - start_point;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving) {
            if(timer <= seconds) {
                timer += Time.deltaTime;
                percent = timer / seconds;

                transform.position = start_point + diff * percent;
            } else {
                if(!transformed) {
                    // Transform();
                }
                transform.position = transform.position + new Vector3(0, 0, -1 * speed);
                if (transform.position.z < 0) {
                    Destroy(gameObject);
                }
            }
        }
        if(!spawnAnim) {
            int rand = Random.Range(0, 4);
            float xSpeed = rotationSpeed;
            float zSpeed = rotationSpeed;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + xSpeed, 0,  transform.rotation.eulerAngles.z + zSpeed);
            totRot += rotationSpeed;
            if(totRot > 180) {
                spawnAnim = true;
            }
        }
    }

    public void Transform() {
        try {
            GetComponent<Renderer>().material.color = new Color(0, 0, 0);
        } catch {
            // Debug.Log("object does not have a renderer, is it complex?");
        }
        transformed = true;
    }

    public void Setup(float seconds, Vector3 end_point) {
        this.seconds = seconds;
        this.start_point = transform.position;
        this.end_point = end_point;
        this.diff = this.end_point - this.start_point;
        this.moving = true;
    }
}
