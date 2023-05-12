using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // To be placed in subclass of player
    // Handles collision
    // Health System reference, to damage player when collisions happen
    public HealthSystem health;
    // Crystal Heart SYstem reference
    public CrystalHeart crystalHeart;
    // Audio Source References to play sound
    public AudioSource hitSound;
    public AudioSource healthSound;
    public AudioSource deathSound;

    public AudioSource crystalSound; 

    // On Trigger Enter
    // Called when a note enters the "trigger" hitbox of the player
    // Figures out appropriate action to take based on the note type
    // Damages player however best fits (or heals them)
    // Plays appropriate sound to provide audible feedback
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name.Contains("Health")) {
            bool result = health.Restore();
            if(result) {
                healthSound.Play();
            }
        } else if(other.gameObject.name.Contains("Crystal")) {
            crystalHeart.AddCrystal();
            crystalSound.Play();
        }
        else if(other.gameObject.name.Contains("Death")) {
            bool result = health.Kill();
            if(result) {
                deathSound.Play();
            }
        } else {
            bool result = health.Damage();
            if(health.GetHealth() > 0 && !health.IsInvulnerable()) {
                hitSound.Play();
            }
            if(health.GetHealth() == 0 && result) {
                deathSound.Play();
            }
        }
    }
    // For resetting the game, stop playing the death sound
    public void Reset() {
        deathSound.Stop();
    }
}
