using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 50.0f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        horizontalInput = Keyboard.current.aKey.isPressed ? -1f :
                          Keyboard.current.dKey.isPressed ? 1f : 0f;
    }

    private void FixedUpdate()
    {
        if (rb != null)
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            Debug.Log("🔴 BOMB HIT!");

            // ✅ Play sound
            if (AudioManager.instance != null)
                AudioManager.instance.PlayBombHit();

            // ✅ Call BombHit() - handles Hard mode instant death
            if (GameManager.instance != null)
                GameManager.instance.BombHit();

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Gem"))
        {
            if (AudioManager.instance != null)
                AudioManager.instance.PlayGemCollect();

            if (GameManager.instance != null)
                GameManager.instance.AddScore(10);

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            if (AudioManager.instance != null)
                AudioManager.instance.PlayCoinCollect();

            if (GameManager.instance != null)
                GameManager.instance.AddScore(5);

            Destroy(collision.gameObject);
        }
    }
}