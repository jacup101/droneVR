using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPredictor : MonoBehaviour
{
    List<GameObject> collidingWith;
    public GameObject leftIndicator;
    public GameObject rightIndicator;


    // Start is called before the first frame update
    void Start()
    {
        collidingWith = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        collidingWith.Add(other.gameObject);
        if(collidingWith.Count > 0) {
            leftIndicator.SetActive(true);
            rightIndicator.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        collidingWith.Remove(other.gameObject);
        if(collidingWith.Count == 0) {
            leftIndicator.SetActive(false);
            rightIndicator.SetActive(false);
        }
    }
}
