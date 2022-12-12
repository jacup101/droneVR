using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPredictor : MonoBehaviour
{
    // List of all objects currently in the "raycast" hitbox of the collision predict1or
    List<GameObject> collidingWith;
    // HUD Indicators to pop on screen when appropriate
    public GameObject healthIndicator;
    public GameObject warningIndicator;
    public GameObject deathIndicator;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the list
        collidingWith = new List<GameObject>();
    }
    // Add a note to the list and update classification, if it collides with the hitbox
    private void OnTriggerEnter(Collider other) {
        collidingWith.Add(other.gameObject);
        SetHud(ClassifyList(collidingWith));
    }
    // Remove a note from list and update classification, if it collides with the hitbox
    private void OnTriggerExit(Collider other) {
        collidingWith.Remove(other.gameObject);
        
        SetHud(ClassifyList(collidingWith));
    }
    // Called upon a game restart
    // Resets HUD and colliding list
    public void Reset() {
        SetHud("none");
        collidingWith = new List<GameObject>();
    }
    // Resets HUD to be empty
    public void ResetHud() {
        healthIndicator.SetActive(false);
        warningIndicator.SetActive(false);
        deathIndicator.SetActive(false);
    }
    // Resets HUD and places the appropriate indicator based on the provided classification
    public void SetHud(string classification) {
        ResetHud();
        if(classification.Equals("none")) {
            // We're good (already reset)
        } else if (classification.Equals("health")) {
            healthIndicator.SetActive(true);
        } else if (classification.Equals("regular")) {
            warningIndicator.SetActive(true);
        } else if (classification.Equals("death")) {
            deathIndicator.SetActive(true);
        }
    }
    // Checks what objects are in the collision predictor hitbox (if any) and returns a "classification" which corresponds to a HUD indicator type 
    public string ClassifyList(List<GameObject> collidingWith) {
        string type = "none";
        if(collidingWith.Count == 0) {
            return "none";
        }
        foreach(GameObject coll in collidingWith) {
            if(type.Equals("none") && coll.name.Contains("Health")) {
                type = "health";
            }
            if((type.Equals("none") || type.Equals("health")) && (!coll.name.Contains("Health") && !coll.name.Contains("Death"))) {
                type = "regular";
            }
            if(coll.name.Contains("Death")) {
                type = "death";
            }
        }
        return type;
    }
}