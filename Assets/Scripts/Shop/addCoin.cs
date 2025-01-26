using UnityEngine;

public class addCoin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            // Score erh?hen ?ber den ScoreManager
            ScoreManager.instance.AddScore(100);
            Destroy(gameObject);
        }
    }
}
