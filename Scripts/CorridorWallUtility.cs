using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CorridorWallUtility : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> leftSide;
    [SerializeField]
    List<GameObject> rightSide;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor() {
        
        float randRed = Random.Range(0.0f, 1.0f);
        float randBlue = Random.Range(0.0f, 1.0f);
        float randGreen = Random.Range(0.0f, 1.0f);

        Color color = new Color(randRed, randBlue, randGreen);
        ChangeColorForList(color, leftSide);
        ChangeColorForList(color, rightSide);
    }

    void ChangeColorForList(Color c, List<GameObject> objList) {
        foreach(GameObject obj in objList) {
            obj.GetComponent<Renderer>().material.color = c;

        }
    }

    public void RandomlyRemovePanels() {
        RemoveRandomlyFromList(Random.Range(0, leftSide.Count / 2), leftSide);
        RemoveRandomlyFromList(Random.Range(0, rightSide.Count/ 2), rightSide);
    }

    void RemoveRandomlyFromList(int count, List<GameObject> objList) {
        for(int i = 0; i < count; i++) {
            int randomIndex = Random.Range(0, objList.Count);
            objList[randomIndex].SetActive(false);
        }
    }

    public void Explode() {
        ExplodeList(leftSide);
        ExplodeList(rightSide);
    }

    void ExplodeList(List<GameObject> objList) {
        foreach(GameObject obj in objList) {
            var collider = obj.GetComponent<Collider>();
            collider.enabled = true;
            var rb = obj.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce(Random.onUnitSphere * 1000);
        }
    }
}
