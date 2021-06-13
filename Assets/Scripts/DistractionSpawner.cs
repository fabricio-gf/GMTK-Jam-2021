using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DistractionSpawner : MonoBehaviour {
    public GameObject prefab;

    [Range(-360, 360)]
    public int rotationAngle;
    private Quaternion rotation;

    [Range(0.0f, 1.0f)]
    public float spawnProbability;
    public Transform spawnPoint;

    private bool didSpawn;

    private void Awake() {
        didSpawn = false;
        rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
        Debug.Assert(prefab != null, "No prefab set");
        Debug.Assert(spawnPoint != null, "No spawn point set");
    }

    private void OnTriggerEnter(Collider other) {
        // Race condition
        if (didSpawn) return;

        if (other.CompareTag("Player")) {
            if (Random.value < spawnProbability) {
                SpawnDistraction();
            }
        }
    }

    private void SpawnDistraction() {
        Instantiate(prefab, spawnPoint.position, rotation);

        // Prevent race condition in the collision frame
        didSpawn = true;

        // Prevent checks in further frames
        Destroy(this);
    }

    private void OnDrawGizmosSelected() {
        if (spawnPoint != null) {
            Gizmos.DrawRay(spawnPoint.position, Quaternion.Euler(0.0f, rotationAngle, 0.0f) * Vector3.forward);
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(spawnPoint.position, 0.1f * Vector3.one);
        }
    }
}
