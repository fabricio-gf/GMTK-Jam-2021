using UnityEngine;

public class RandomPickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float spawnChance = 0.5f;

    private void OnEnable()
    {
        RoundManager.instance._onRoundStart += SpawnSticks;
    }

    private void OnDisable()
    {   
        RoundManager.instance._onRoundStart -= SpawnSticks;
    }

    public void SpawnSticks()
    {
        foreach (Transform spawnPoint in transform)
        {
            if(Random.value <= spawnChance)
            {
                Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
                Vector3 position = spawnPoint.position + new Vector3(randomPos.x, 0f, randomPos.y);
                Instantiate(pickupPrefab, position, Quaternion.identity);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        foreach(Transform child in transform)
        {
            Gizmos.DrawWireSphere(child.position, spawnRadius);
        }
    }
}
