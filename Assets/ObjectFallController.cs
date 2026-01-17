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

     
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        screenLeft = cam.transform.position.x - halfWidth + margin;
        screenRight = cam.transform.position.x + halfWidth - margin;
        spawnHeight = cam.transform.position.y + halfHeight + 1f;

        InvokeRepeating(nameof(SpawnObject), 0f, spawnRate);
    }

    void SpawnObject()
    {
      
        int randomIndex = Random.Range(0, fallingPrefabs.Length);

        float randomX = Random.Range(screenLeft, screenRight);
        Vector3 spawnPos = new Vector3(randomX, spawnHeight, 0f);

        GameObject obj = Instantiate(
            fallingPrefabs[randomIndex],
            spawnPos,
            Quaternion.identity
        );

        // Add falling behaviour
        SmoothFall fall = obj.AddComponent<SmoothFall>();
        fall.fallSpeed = fallSpeed;
    }
}

public class SmoothFall : MonoBehaviour
{
    [HideInInspector] public float fallSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < - 12f)
        {
            Destroy(gameObject);
        }
    }
}
