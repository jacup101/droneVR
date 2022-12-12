using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Handles the health of the player
    // Health related variables
    int maxHealth = 3;
    int health = 3;
    // Health Indicators
    [SerializeField]
    GameObject warning;
    [SerializeField]
    GameObject serious;
    // Rigidbody of the player
    Rigidbody rb;
    // State variables
    [SerializeField]
    bool invulnerable = false;
    bool simpleVisual = false;
    public bool globalInvulnerable = false;

    void Start()
    {
        // Get the associated rigidbody
        rb = GetComponent<Rigidbody>();
    }
    // Public method to damage the player and handle any corresponding actions
    public bool Damage() {
        if(health == 0 ) {
            return false;
        }
        if(!invulnerable) {
            health--;
        }
        HandleHealth(health);
        return true;
    }
    // Public method to heal the player and handle any corresponding actions
    public bool Restore() {
        if(health == 3) {
            return false;
        }
        else health ++;
        HandleHealth(health);
        return true;
    }
    // Sets indicators based on the currrent health of the player. Also handles death
    void HandleHealth(int health) {
        if(invulnerable) {
            return;
        }
        switch(health) {
            case 3:
                warning.SetActive(false);
                serious.SetActive(false);
                break;
            case 2:
                warning.SetActive(true);
                serious.SetActive(false);
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
    // Public method to immediately kill the player from another class, regardless of current health
    public bool Kill() {
        if (health == 0 || invulnerable) {
            return false;
        }
        health = 0;
        HandleHealth(health);
        return true;
    }
    // Method called when the player dies, calls all systems' death method
    void Die() {
        if(!simpleVisual) {
            this.GetComponent<Collider>().isTrigger = false;
            rb.useGravity = true;
            rb.AddRelativeForce(Random.onUnitSphere * 5);
        }
        this.GetComponent<RhythmSystem>().Die();
        this.GetComponent<MotionControls>().Die();
        this.GetComponent<ControllerControls>().Die();
    }
    // Exposes health other classes
    public int GetHealth() {
        return health;
    }
    // Starts invulnerability for player
    public void StartInvulnerable() {
        this.invulnerable = true;
    }
    // Ends invulnerability for player (if not global invulnerable)
    public void EndInvulnerable() {
        if(!globalInvulnerable) {
            this.invulnerable = false;
        }
    }
    // Returns whether or not the player is invulnerable or not
    public bool IsInvulnerable() {
        return invulnerable;
    }
    // Sets global invulnerability (for dev options)
    public void SetGlobalInvulnerability(bool val) {
        globalInvulnerable = val;
        invulnerable = val;
    }
    // Reset method called when game is restarted
    public void Reset() {
        this.GetComponent<Collider>().isTrigger = true;
        this.health = maxHealth;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        warning.SetActive(false);
        serious.SetActive(false);
    }
    // Sets accessibility mode of the health system (for when player loses)
    public void SetAccessibility(bool mode) {
        simpleVisual = mode;
    }
}

