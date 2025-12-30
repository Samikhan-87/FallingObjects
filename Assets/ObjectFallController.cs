using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    [SerializeField] GameObject fallingObjectPrefab;
    [SerializeField] float spawnRate = 1.0f; 
    [SerializeField] float spawnRangeX = 10f;
    [SerializeField] float spawnHeight = 10f;
    [SerializeField] float fallSpeed = 5f; 

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 0f, spawnRate);
    }

    void SpawnObject()
    {
        GameObject obj = Instantiate(
            fallingObjectPrefab,
            new Vector3(Random.Range(-spawnRangeX, spawnRangeX), spawnHeight, 0f),
            Quaternion.identity
        );

       
        SmoothFall fallScript = obj.AddComponent<SmoothFall>();
        fallScript.fallSpeed = fallSpeed;
    }
}

public class SmoothFall : MonoBehaviour
{
    [HideInInspector] public float fallSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }
}
