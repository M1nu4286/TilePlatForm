using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    float gravityScaleAtStart;
    BoxCollider2D  myFeetCollider;

    bool isAlive=true;

    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick= new Vector2(10f,20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart=myRigidbody.gravityScale;
        myFeetCollider=GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(!isAlive){return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }
    void OnMove(InputValue value)
    {
         if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * MoveSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunnung", hasHorizontalSpeed);
    }
    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }

    }
    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale=gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        myRigidbody.gravityScale=0f;
        Vector2 ClimbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y*climbSpeed );
        myRigidbody.linearVelocity = ClimbVelocity;

         bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
         myAnimator.SetBool("isClimbing", hasVerticalSpeed);
     
    }

    void OnAttack(InputValue value)
    {
        if(!isAlive){return;}
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Die()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies","Spike")))
        {
            isAlive=false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity=deathKick;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
  
}
