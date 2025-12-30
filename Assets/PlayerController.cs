using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    float horizontalInput;
    float moveSpeed = 30.0f;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    
    void Update()
    {
        horizontalInput = Keyboard.current.aKey.isPressed ? -1f :
                          Keyboard.current.dKey.isPressed ? 1f : 0f;
        Debug.Log(horizontalInput);

    }


    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            Destroy(this.gameObject);
        }
    }
}
