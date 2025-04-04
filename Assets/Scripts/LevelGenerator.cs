using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] levelPart;
    [SerializeField] private Vector3 nextPartPosition;
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        DestroyPlatform();
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        while (Vector2.Distance(player.transform.position, nextPartPosition) < distanceToSpawn)
        {
            Transform part = levelPart[Random.Range(0, levelPart.Length)];

            Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("startPoint").position.x, 0);

            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);

            nextPartPosition = newPart.Find("endPoint").position;
        }
    }

    private void DestroyPlatform()
    {
        if(transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);
            if(Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete) {
                Destroy(partToDelete.gameObject);
            }
        }
    }
}
