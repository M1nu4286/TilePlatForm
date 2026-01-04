using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;

    [SerializeField] float bulletSpeed=20f;
    // rigirt is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody=GetComponent<Rigidbody2D>();
        player=FindFirstObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x*bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.linearVelocity=new Vector2(xSpeed,0f);  
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject,1f);
    }
}
