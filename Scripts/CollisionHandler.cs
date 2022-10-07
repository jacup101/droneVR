using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // TO be placed in subclass of player
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        rb.useGravity = true;
        rb.AddRelativeForce(Random.onUnitSphere * 5);
        transform.parent.GetComponent<RhythmSystem>().Die();
        transform.parent.GetComponent<MotionControls>().Die();
    }
}
