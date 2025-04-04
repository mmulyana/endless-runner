using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    private int amountOfCoins;
    [SerializeField] private GameObject coinPrefab;

    [SerializeField] private int minCoins;
    [SerializeField] private int maxCoins;
    [SerializeField] private float changeToSpawn;

    void Start()
    {
        amountOfCoins = Random.Range(minCoins, maxCoins);

        for(int i = 0; i < amountOfCoins; i++)
        {
            bool canSpawn = changeToSpawn > Random.Range(0, 100);
            Vector3 offset = new Vector2(i, 0);

            if(canSpawn)
            {
                Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);
            }
        }    
    }
}
