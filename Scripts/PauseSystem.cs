using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    // Responsible for pausing all parts of the game, just split into its own system for organization purposes
    // State variables
    public bool paused = false;
    // UI Game Objects
    public GameObject menuCanvas;
    public GameObject laser;
    // Cooldown to avoid flickering
    int pauseCooldown = 0;
    // Other systems that must be paused
    public MotionControls mc;
    public RhythmSystem rs;
    public ControllerControls cc;

    void Update()
    {
        if (pauseCooldown > 0) {
            pauseCooldown--;
        }
    }

    // Attempts to pause or unpause the game, based on the cooldown
    public void TryPause(int cooldown) {
        bool changed = false;
        if (paused) {
            menuCanvas.SetActive(false);
            laser.SetActive(false);
            mc.Unpause();
            rs.Unpause();
            cc.Unpause();
            paused = false;
            changed = true;
            pauseCooldown = cooldown;
        }
        if (!paused && !changed) {
            menuCanvas.SetActive(true);
            laser.SetActive(true);
            mc.Pause();
            rs.Pause();
            cc.Pause();
            paused = true;
            pauseCooldown = cooldown;
        }
    }

    public int GetPauseCooldown() {
        return pauseCooldown;
    }
    // Resets pause when game is restarted
    public void Reset() {
        mc.Unpause();
        rs.Unpause();
        cc.Unpause();
        this.paused = false;
        pauseCooldown = 0;
    }
}
