using UnityEngine;

public class Trap : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().Damage();
        }
    }
}
