using UnityEngine;

public class addCoin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}
