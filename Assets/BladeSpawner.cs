using UnityEngine;

public class BladeSpawner : MonoBehaviour
{
    public static BladeSpawner instance;
    [SerializeField] GameObject bladePrefab;
    [SerializeField] float spawnRate = 2f;

    // Screen margins (same as your ObjectFallController)
    private float screenLeft;
    private float screenRight;
    [SerializeField] float margin = 0.5f; // Adjust this in Inspector

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Calculate screen boundaries
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        screenLeft = bottomLeft.x + margin;
        screenRight = topRight.x - margin;
    }

    public void EnableBlades()
    {
        InvokeRepeating(nameof(SpawnBlade), 1f, spawnRate);
    }

    public void DisableBlades()
    {
        CancelInvoke(nameof(SpawnBlade));
    }

    void SpawnBlade()
    {
        // Get the top of the screen for Y position
        float spawnY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Random X position between screen left and right (with margin)
        float randomX = Random.Range(screenLeft, screenRight);

        Vector3 pos = new Vector3(randomX, spawnY, 0f);
        Instantiate(bladePrefab, pos, Quaternion.identity);
    }
}