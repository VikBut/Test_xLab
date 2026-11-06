using UnityEngine;
public class SpawnOnKey : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;
    public float destroyTime = 3f;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Destroy(obj, destroyTime);
            
            
        }
    }
    
}
