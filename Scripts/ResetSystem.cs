using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSystem : MonoBehaviour
{
    // Responsible for resetting all parts of the game, just split into its own system for organization purposes
    // Systems to Reset
    public RhythmSystem rs;
    public CorridorHandler ch;
    public CollisionHandler colh;
    public MotionControls mc;
    public ControllerControls cc;
    public HealthSystem hs;
    public CollisionPredictor cp;
    public PauseSystem ps;
    // Resets the game with a specific level, for when resetting is done from main menu
    public void ResetGame(string level) {
        rs.ResetGame(level);
        ResetCommon();
    }
    // Resets the game with current level, for when resetting is done using the 'X' button
    public void ResetGame() {
        
        rs.ResetGame();
        ResetCommon();
        
    }
    // Method in common with both ResetGame methods, which resets all systems
    public void ResetCommon() {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        colh.Reset();
        ch.ResetGame();
        mc.Reset();
        cc.Reset();
        hs.Reset();
        cp.Reset();
        ps.Reset();
    }
}
