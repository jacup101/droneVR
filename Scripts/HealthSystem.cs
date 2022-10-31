using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    int health = 3;
    [SerializeField]
    GameObject warning;
    [SerializeField]
    GameObject serious;
    Rigidbody rb;
    [SerializeField]
    bool invulnerable = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage() {
        health--;
        HandleHealth(health);
    }
    
    void HandleHealth(int health) {
        if(invulnerable) {
            return;
        }
        switch(health) {
            case 2:
                warning.SetActive(true);
                break;
            case 1:
                warning.SetActive(false);
                serious.SetActive(true);
                break;
            case 0:
                serious.SetActive(false);
                Die();
                break;
        }
    }

    void Die() {
        this.GetComponent<Collider>().isTrigger = false;
        rb.useGravity = true;
        rb.AddRelativeForce(Random.onUnitSphere * 5);
        this.GetComponent<RhythmSystem>().Die();
        this.GetComponent<MotionControls>().Die();
    }

    public int GetHealth() {
        return health;
    }
}

