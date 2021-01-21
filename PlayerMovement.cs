using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //necessary 4 anims and physics
    private Rigidbody2D rb2d;
    private Animator myAnimator;

    private bool facingRight = true;

    //vars to play with
    public float speed = 2.0f;
    public float horizMovement; //=1[OR]-1[OR]0

    private void Start()
    {
        //define the game objs find on the player
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    //Handles the input for physics
    private void Update()
    {
        //check direction given by player
        horizMovement = Input.GetAxisRaw("Horizontal");
    }

    //handles running the physics
    private void FixedUpdate()
    {
        //move the character left and right
        rb2d.velocity = new Vector2(horizMovement * speed, rb2d.velocity.y);
        Flip(horizMovement);
        myAnimator.SetFloat("floatSpeed", Mathf.Abs(horizMovement));
    }

    //flipping func
    private void Flip(float horizontal)
    {
        if  (horizontal < 0 && facingRight || horizontal > 0 && !facingRight) 
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

}
