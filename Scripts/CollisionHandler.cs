using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // TO be placed in subclass of player
    HealthSystem health;

    AudioSource hitSound;
    // Start is called before the first frame update
    void Start()
    {
        health = transform.parent.GetComponent<HealthSystem>();
        hitSound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        health.Damage();
        if(health.GetHealth() > 0) {
            hitSound.Play();
        }
    }
}
