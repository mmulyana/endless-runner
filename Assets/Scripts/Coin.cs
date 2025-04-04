using UnityEngine;

public class coins : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //  collision with tag
        //  if(collision.tag == "Player")
        //  {
        //      Destroy(gameObject);
        //  }

        if(collision.GetComponent<Player>() != null)
        {
            GameManager.instance.coins++;
            Destroy(gameObject);
        }
    }
}
