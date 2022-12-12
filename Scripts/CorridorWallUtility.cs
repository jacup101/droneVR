using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CorridorWallUtility : MonoBehaviour
{
    // Class given to each section of the corridor, allowing it to be manipulated based on functions defined in this class
    // List of subobjects
    [SerializeField]
    List<GameObject> leftSide;
    [SerializeField]
    List<GameObject> rightSide;
    // Changes colors of sub game objects (currently random, but can be changed to be specific in the future)
    public void ChangeColor() {
        
        float randRed = Random.Range(0.0f, 1.0f);
        float randBlue = Random.Range(0.0f, 1.0f);
        float randGreen = Random.Range(0.0f, 1.0f);

        Color color = new Color(randRed, randBlue, randGreen);
        ChangeColorForList(color, leftSide);
        ChangeColorForList(color, rightSide);
    }
    // Helper method to change colors in list, private
    void ChangeColorForList(Color c, List<GameObject> objList) {
        foreach(GameObject obj in objList) {
            obj.GetComponent<Renderer>().material.color = c;

        }
    }
    // Removes panels randomly from both sublists
    public void RandomlyRemovePanels() {
        RemoveRandomlyFromList(Random.Range(0, leftSide.Count / 2), leftSide);
        RemoveRandomlyFromList(Random.Range(0, rightSide.Count/ 2), rightSide);
    }
    // Helper method to remove panels
    void RemoveRandomlyFromList(int count, List<GameObject> objList) {
        for(int i = 0; i < count; i++) {
            int randomIndex = Random.Range(0, objList.Count);
            objList[randomIndex].SetActive(false);
        }
    }
    // Explodes both lists
    public void Explode() {
        ExplodeList(leftSide);
        ExplodeList(rightSide);
    }
    // "Falls" both lists (for accessibility mode)
    public void Fall() {
        FallList(leftSide);
        FallList(rightSide);
    }
    // Explodes an individual list of corridor objects
    void ExplodeList(List<GameObject> objList) {
        foreach(GameObject obj in objList) {
            var collider = obj.GetComponent<Collider>();
            collider.enabled = true;
            var rb = obj.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce(Random.onUnitSphere * 1000);
        }
    }
    // "Falls an individual list of corridor objects (for accessibility mode)
    void FallList(List<GameObject> objList) {
        foreach(GameObject obj in objList) {
            var collider = obj.GetComponent<Collider>();
            collider.enabled = true;
            var rb = obj.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }
}
