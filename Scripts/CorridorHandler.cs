using System.Collections.Generic;
using UnityEngine;

public class CorridorHandler : MonoBehaviour
{
    // Handles the corridor visuals of the game
    // List of both currently spawned corridors as well as old ones
    List<GameObject> corridors;
    // Old corridors is used doing restarts, to make exploded corridors are properly removed
    List<GameObject> oldCorridors;
    // Prefabs of the corridors that can spawn, and related variables
    public List<GameObject> cooridorPrefabs;
    public int prefabIndex = 0;
    public GameObject lastCorridor;
    // Variables defined by the definitions json
    float zStart = 0;
    float speed = 0;
    float rotationSpeed = 0;
    // State variables
    bool spawning = false;
    string type = "random";
    public bool paused = false;
    // Random rotations helper
    float[] rots = new float[]{0f, 30f, 45f, 60f, 90f};

    // Start is called before the first frame update
    void Start()
    {
        corridors = new List<GameObject>();
        oldCorridors = new List<GameObject>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!paused) {
            MoveAllCorridors();
        }
    }
    // Handles movement of every corridor
    void MoveAllCorridors() {
        // Move them in z direction
        List<GameObject> toDie = new List<GameObject>();
        foreach(GameObject obj in corridors) {
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z - speed);
            // Add game objects to a temporary list if they are out of bounds
            if(obj.transform.position.z < -1) {
                toDie.Add(obj);
            }
            // Rotate the corridor if it is of a rotating type
            if(type.Contains("rotating")) {
                obj.transform.Rotate(new Vector3(rotationSpeed, 0, 0), Space.Self);
            }
        }
        // If the game objects are out of bounds, remove them from scene
        foreach(GameObject dead in toDie) {
            Die(dead);
        }
        // Spawn a new corridor once the previous one has reached a certain position
        if(lastCorridor != null && lastCorridor.transform.position.z <= zStart - 1) {
            if(spawning) {
                SpawnCorridor();
            }
        }
    }
    // Spawns a new corridor when called
    void SpawnCorridor() {
        float xRot = 0;
        // Change the permanent rotation randomly if called for
        if(type.Contains("randrot")) {
            xRot = rots[Random.Range(0, rots.Length)];
        }
        lastCorridor = GameObject.Instantiate(cooridorPrefabs[prefabIndex], new Vector3(0,0, zStart), Quaternion.Euler(xRot, 90, 0));
        // lastCorridor.GetComponent<CorridorWallUtility>().RandomlyRemovePanels();
        // Change the color, if called for
        if(type.Contains("color")) {
            lastCorridor.GetComponent<CorridorWallUtility>().ChangeColor();
        } 
        // Remove panels randomly, if called for
        if(type.Contains("random")) {
            lastCorridor.GetComponent<CorridorWallUtility>().RandomlyRemovePanels();
        }
        corridors.Add(lastCorridor);
    }
    // Begins spawning corridors when called, with the appropriate properties
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
        // Assign the appropriate rotation direction
        if(type.Contains("left")) {
            this.rotationSpeed = -1;
        }
        if(type.Contains("right")) {
            this.rotationSpeed = 1;
        }
        SpawnCorridor();
        this.spawning = true;
    }
    public void HandleEvent(float zstart, float speed, string type) {
        if(this.spawning) {
            ChangeCorridor(type);
        } else {
            StartCorridor(zstart, speed, type);
        }
    }
    public void ChangeCorridor(string type) {
        this.type = type;
        // Assign the prefab
        if(type.Contains("tunnel")) {
            this.prefabIndex = 0;
        }
        if(type.Contains("accent")) {
            this.prefabIndex = 1;
        }
        // Assign the appropriate rotation direction
        if(type.Contains("left")) {
            this.rotationSpeed = -1;
        }
        if(type.Contains("right")) {
            this.rotationSpeed = 1;
        }
    }
    // Stop the spawning of corridors
    public void StopCorridor() {
        this.spawning = false;
    }
    // Explode the corridor when a loss occurs
    public void Explode() {
        this.spawning = false;
        for(int i = corridors.Count - 1; i >= 0; i --) {
            GameObject obj = corridors[i];
            obj.GetComponent<CorridorWallUtility>().Explode();
            oldCorridors.Add(obj);
            corridors.RemoveAt(i);
        }
    }
    // Make the corridor fall, used for accesibility mode in place of explode
    public void Fall() {
        this.spawning = false;
        for(int i = corridors.Count - 1; i >= 0; i --) {
            GameObject obj = corridors[i];
            obj.GetComponent<CorridorWallUtility>().Fall();
            oldCorridors.Add(obj);
            corridors.RemoveAt(i);
        }
    }
    // Resets the corridors in preparation for a new game
    public void ResetGame() {
        this.spawning = false;
        this.paused = false;
        // Remove corridors currently in place
        for(int i = corridors.Count - 1; i >= 0; i --) {
            GameObject obj = corridors[i];
            corridors.RemoveAt(i);
            Destroy(obj);
        }
        // Remove previously exploded corridors
        for(int i = oldCorridors.Count - 1; i >= 0; i --) {
            GameObject obj = oldCorridors[i];
            oldCorridors.RemoveAt(i);
            Destroy(obj);
        }
    }
    // Die removes a specific corridor
    void Die(GameObject cor) {
        corridors.Remove(cor);
        GameObject.Destroy(cor);
    }
    // Pauses the game when called
    public void Pause() {
        paused = true;
    }
    // Unpauses the game when called
    public void Unpause() {
        paused = false;
    }
}
