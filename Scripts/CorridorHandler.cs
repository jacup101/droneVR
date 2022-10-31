using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorHandler : MonoBehaviour
{

    List<GameObject> corridors;
    public List<GameObject> cooridorPrefabs;

    public int prefabIndex = 0;
    public GameObject lastCorridor;
    float zStart = 0;
    float speed = 0;
    float rotationSpeed = 0;
    bool spawning = false;
    string type = "random";
    // Random rotations helper
    float[] rots = new float[]{0f, 30f, 45f, 60f, 90f};

    // Start is called before the first frame update
    void Start()
    {
        corridors = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveAllCorridors();
    }

    void MoveAllCorridors() {
        // Move them in z direction
        List<GameObject> toDie = new List<GameObject>();
        foreach(GameObject obj in corridors) {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z - speed);
            if(obj.transform.position.z < -1) {
                toDie.Add(obj);
            }
            if(type.Contains("rotating")) {
                obj.transform.Rotate(new Vector3(rotationSpeed, 0, 0), Space.Self);
            }
        }
        foreach(GameObject dead in toDie) {
            Die(dead);
        }
        if(lastCorridor != null && lastCorridor.transform.position.z <= zStart - 1) {
            if(spawning) {
                SpawnCorridor();
            }
        }
    }
    void SpawnCorridor() {
        float xRot = 0;
        if(type.Contains("randrot")) {
            xRot = rots[Random.Range(0, rots.Length)];
        }
        lastCorridor = GameObject.Instantiate(cooridorPrefabs[prefabIndex], new Vector3(0,0, zStart), Quaternion.Euler(xRot, 90, 0));
        // lastCorridor.GetComponent<CorridorWallUtility>().RandomlyRemovePanels();
        if(type.Contains("color")) {
            lastCorridor.GetComponent<CorridorWallUtility>().ChangeColor();
        } if(type.Contains("random")) {
            lastCorridor.GetComponent<CorridorWallUtility>().RandomlyRemovePanels();
        }

        corridors.Add(lastCorridor);
    }

    // TODO Assign Index
    public void StartCorridor(float zstart, float speed, string type) {
        this.zStart = zstart;
        this.speed = speed;
        this.type = type;
        // Assign the prefab
        if(type.Contains("tunnel")) {
            this.prefabIndex = 0;
        }
        if(type.Contains("accent")) {
            this.prefabIndex = 1;
        }
        if(type.Contains("left")) {
            this.rotationSpeed = -1;
        }
        if(type.Contains("right")) {
            this.rotationSpeed = 1;
        }
        SpawnCorridor();
        this.spawning = true;
    }

    public void StopCorridor() {
        this.spawning = false;
    }

    public void Explode() {
        this.spawning = false;
        for(int i = corridors.Count - 1; i >= 0; i --) {
            GameObject obj = corridors[i];
            obj.GetComponent<CorridorWallUtility>().Explode();
            corridors.RemoveAt(i);
        }
    }

    void Die(GameObject cor) {
        corridors.Remove(cor);
        GameObject.Destroy(cor);
    }
}
