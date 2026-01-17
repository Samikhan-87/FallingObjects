using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] float fallSpeed = 4f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Destroy if goes off screen
        if (transform.position.y < -12f)
            Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("⚡ Blade collision with: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

        // Only react to Player or Ground
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("🗡️ BLADE HIT PLAYER!");

            // ✅ Play sound with null check
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayBladeHit();
                Debug.Log("Playing blade hit sound...");
            }
            else
            {
                Debug.LogError("❌ AudioManager is NULL when blade hit!");
            }

            // Apply damage based on difficulty
            if (GameManager.instance != null)
            {
                if (GameManager.instance.currentDifficulty == GameDifficulty.Hard)
                {
                    GameManager.instance.InstantDeath();
                }
                else
                {
                    GameManager.instance.ReduceTime(10f);
                }
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("Blade hit platform");
            // Hit ground - just destroy
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("⚠️ Blade hit unrecognized tag: " + collision.gameObject.tag);
        }
    }
}