using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMethods : MonoBehaviour
{
    // Controls the options in the Main Menu UI
    // System References
    public RhythmSystem rs;
    public HealthSystem hs;
    // State variables
    public bool spawnEvents = true;
    public bool spawnNotes = true;
    public bool globalInvulnerable = false;
    public bool accessibility = false;
    // Turn events on or off (visual events)
    public void ToggleEvents() {
        spawnEvents = !spawnEvents;
        rs.spawnEvents = spawnEvents;
    }
    // Turn notes on or off 
    public void ToggleNotes() {
        spawnNotes = !spawnNotes;
        rs.spawnNotes = spawnNotes;
    }
    // Turn invulnerability on or off
    public void ToggleInvulnerability() {
        globalInvulnerable = !globalInvulnerable;
        hs.SetGlobalInvulnerability(globalInvulnerable);
    }
    // Turn accessibility mode on or off
    public void ToggleAccessibility () {
        accessibility = !accessibility;
        hs.SetAccessibility(accessibility);
        rs.SetAccessibility(accessibility);
    }
}