using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleData : MonoBehaviour
{
    public float obstacleHealth;
    public string attackedSoundName;
    public string deathSoundName;

    public GameObject deathParticle;

    public planeSlot obstaclePlaneSlot;
    private void Start()
    {
        // obstaclePlaneSlot = new planeSlot(icon: icon, 99);
    }

    public void AttackObstacle(float damage)
    {
        obstacleHealth -= damage;
        if (obstacleHealth <= 0)
            KillObstacle();
        else
        {
            // Play sound effect and animation
        }
    }

    public void KillObstacle()
    {
        Debug.Log($"Killing obstacle: {gameObject.name}");
        // Play death sound effect
        // spawn an appropriate particle effect, deparent so it's not deleted
        // Delete obstacle
        if (deathSoundName != "")
            AudioManager.instance.Play(deathSoundName);
        Destroy(gameObject);
    }
}
