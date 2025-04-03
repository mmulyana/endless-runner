using Unity.VisualScripting;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Player player;

    private bool canDetected;

    private void Update()
    {
        if(canDetected)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
