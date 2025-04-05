using UnityEngine;

public class MovingTrap : Trap
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform[] movePosition;

    private int i = 0;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePosition[i].position, speed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position, movePosition[i].position) < .25f) {
            i++;

            if(i >= movePosition.Length)
            {
                i = 0;
            }
        }

        if(transform.position.x > movePosition[i].position.x)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        } else
        {
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
