using Unity.VisualScripting;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Player player;

    private bool canDetected;

    BoxCollider2D boxCol => GetComponent<BoxCollider2D>();

    private void Update()
    {
        if(canDetected)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCol.bounds.center, boxCol.size, 0);

        //foreach (var hit in colliders)
        //{
        //    if(hit.gameObject.GetComponent<PlatformController>() != null)
        //    {
        //        return;
        //    }
        //}

        if(collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            canDetected = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            canDetected = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
