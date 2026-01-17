using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject[] fallingPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] float spawnRate = 1.0f;
    [SerializeField] float fallSpeed = 2f;
    [SerializeField] float margin = 0.5f;

    private Camera cam;
    private float screenLeft;
    private float screenRight;
    private float spawnHeight;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera nahi mili!");
            return;
        }

        ApplyDifficultySettings();

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        screenLeft = cam.transform.position.x - halfWidth + margin;
        screenRight = cam.transform.position.x + halfWidth - margin;
        spawnHeight = cam.transform.position.y + halfHeight + 1f;

        InvokeRepeating(nameof(SpawnObject), 0f, spawnRate);
    }

    void ApplyDifficultySettings()
    {
        switch (GameManager.instance.currentDifficulty)
        {
            case GameDifficulty.Easy:
                spawnRate = 1.2f;
                fallSpeed = 2f;
                break;
            case GameDifficulty.Medium:
                spawnRate = 1.0f;
                fallSpeed = 2.5f;
                break;
            case GameDifficulty.Hard:
                spawnRate = 0.7f;   // 🔥 faster spawn
                fallSpeed = 3.2f;   // 🔥 faster fall
                break;
        }
    }

    void SpawnObject()
    {
        GameObject prefabToSpawn = fallingPrefabs[Random.Range(0, fallingPrefabs.Length)];
        float randomX = Random.Range(screenLeft, screenRight);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, 0f);

        GameObject obj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        SmoothFall fall = obj.AddComponent<SmoothFall>();
        fall.fallSpeed = fallSpeed;
    }

    public class SmoothFall : MonoBehaviour
    {
        [HideInInspector] public float fallSpeed = 2f;

        void Update()
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // Destroy if goes below screen
            if (transform.position.y < -12f)
            {
                Destroy(gameObject);
            }
        }

        // ✅ Use OnCollisionEnter2D for ground collision
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                Destroy(gameObject);
            }
        }
    }
}