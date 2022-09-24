using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInTime : MonoBehaviour {

    public float seconds;
    public float timer;
    public float percent;

    public Vector3 start_point;
    public Vector3 end_point;
    public Vector3 diff;
    public bool moving;

    // Start is called before the first frame update
    void Start()
    {
        
        start_point = transform.position;
        
        diff = end_point - start_point;
        moving = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(moving) {
            if(timer <= seconds) {
                timer += Time.deltaTime;
                percent = timer / seconds;

                transform.position = start_point + diff * percent;
            }
        }
    }

    public void Setup(float seconds, Vector3 end_point) {

    }
}
