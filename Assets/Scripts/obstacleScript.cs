using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleScript : MonoBehaviour
{
    public float speedImpact;
    public int scoreAmount = 1;
    public int numberOfBoosts = 1;
    // Start is called before the first frame update
    void Start()
    {
        TreadmillGeneration.instance.obstacles.Add(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TreadmillGeneration.instance.ChangeTreadmillSpeed(speedImpact);
            TreadmillGeneration.instance.obstacles.Remove(this.gameObject);
            ScoreKeeperScript.instance.AddScore(scoreAmount);
            Player.instance.AddSpeedBoost(numberOfBoosts);
            Destroy(gameObject);
        }
    }
}
