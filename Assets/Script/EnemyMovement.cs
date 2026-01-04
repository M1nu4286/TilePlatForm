using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed=1f;
    Rigidbody2D myRigidbody2D;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody2D=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody2D.linearVelocity=new Vector2(moveSpeed,0f);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed= -moveSpeed;
        FlipEnemyFacing();

    }
    void FlipEnemyFacing()
    {
       
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody2D.linearVelocity.x)), 1f);
        
    }
}
