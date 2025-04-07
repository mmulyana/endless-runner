using System.Collections;
using UnityEngine;

public class PlatformFall : MonoBehaviour
{
    public Rigidbody2D rb;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            StartCoroutine(fall());
        }
    }


    private IEnumerator fall()
    {
        yield return new WaitForSeconds(1f);
        rb.gravityScale = 5;
    }
}
