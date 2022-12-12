using UnityEngine;

public class ControllerControls : MonoBehaviour
{
    // State booleans
    public bool dead = false;
    public bool doingBarrelRoll = false;
    public bool paused = true;
    // Barrel roll fields
    int barrelTimer = 0;
    Vector3 origin = new Vector3(0, 0, 0);
    public GameObject barrelIndicator;
    public int barrelCooldownValue;
    float barrelCooldown = 0;
    // System References
    public HealthSystem hs;
    public ResetSystem reset;
    public PauseSystem ps;

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        // Read for the possible inputs from Meta Quest 2 Controllers and take appropriate action
        if (OVRInput.Get(OVRInput.Button.Three) && !paused) {
            reset.ResetGame();
        }
        if (OVRInput.Get(OVRInput.Button.Start) && ps.GetPauseCooldown() == 0) {
            ps.TryPause(15);
        }
        if (OVRInput.Get(OVRInput.Button.One) && !dead && !paused) {
            TryBarrelRoll();
        }
        
        // For debugging purposes. allows controls to be played using M&K i
        #if UNITY_EDITOR
        if (Input.GetKey("space"))
        {
            TryBarrelRoll();
        }
        if (Input.GetKey("escape"))
        {
            ps.TryPause(300);
        }
        if (Input.GetKey("r") && !paused) {
            reset.ResetGame();
        }
        #endif 
        
        // Handle barrel roll cooldown and indicator
        if(barrelCooldown > 0) {
            barrelCooldown -= Time.deltaTime;
        }
        if(barrelCooldown <= 0 && !barrelIndicator.activeSelf) {
            barrelCooldown = 0;
            barrelIndicator.SetActive(true);
        }
        // Perform the barrel roll rotation if it is running
        if(doingBarrelRoll) {
            barrelTimer = BarrelRoll(barrelTimer);
            if(barrelTimer >= 360) {
                doingBarrelRoll = false;
                barrelTimer = 0;
                hs.EndInvulnerable();
            }
        }
    }
    // Attempt to barrel roll, called if the button is pressed, only works when the cooldown is 0
    void TryBarrelRoll() {
        if(barrelCooldown <= 0) {
            hs.StartInvulnerable();
            barrelCooldown = barrelCooldownValue;
            barrelIndicator.SetActive(false);
            doingBarrelRoll = true;
            
        }
    }
    // Helper function to perform the barrel roll
    int BarrelRoll(int timer) {
        int angleRot = 20;
        if (timer < 360) {
            transform.RotateAround(origin, Vector3.forward, angleRot);
        }
        return timer + angleRot;
    }
    // Public function to let this class know that the game has ended
    public void Die() {
        this.dead = true;
    }
    // Reset function for when a restart occurs
    public void Reset() {
        dead = false;
        this.paused = false;
        doingBarrelRoll = false;
        barrelTimer = 0;
        barrelCooldown = 0;
        barrelIndicator.SetActive(true);
    }
    // Pause the game when called
    public void Pause() {
        this.paused = true;
    }
    // Unpause the game when called
    public void Unpause() {
        this.paused = false;
    }
}
