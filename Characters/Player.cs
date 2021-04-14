using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Transform wallCheck;

    private float runSpeed = 1.5f;
    private float walkSpeed = 0.5f;
    public float wallCheckDistance;
    public int maxHealth = 100;
    public bool canWallSlide = false;
    public ChestController CC;

    public bool isMoving = false;
 
    public override void Start()
    {
        base.Start();
        speed = runSpeed;
        currentHealth = maxHealth;
        canAttack = false;
    }

    public override void Update()
    {
        base.Update();
        if(canMove){
            direction = Input.GetAxisRaw("Horizontal");    //getting movement direction from input
        }
        if(direction == 0){
            isMoving = false;
        }
        else{
            isMoving = true;
        }
        HandleLayers();

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);   //checking if there's a wall if front of player
        
        //if player's on the ground he's able to jump
        if (grounded)
        {
            jumpTimeCounter = jumpTime;
            myAnim.ResetTrigger("Jump");
            myAnim.SetBool("Falling", false);
        }
        
        if (isWallSliding)
        {
            jumpTimeCounter = jumpTime;
            myAnim.ResetTrigger("Jump");
            myAnim.SetBool("Falling", false);
        }

        //use Space to jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jump();
            stoppedJumping = false;
            myAnim.SetTrigger("Jump");
        }

        //the longer you hold button - the higher the jump
        if (Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0))
        {
            Jump();
            jumpTimeCounter -= Time.deltaTime;
            myAnim.SetTrigger("Jump");
        }

        //regular weak jump
        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
            myAnim.SetBool("Falling", true);
            myAnim.ResetTrigger("Jump");
        }

        //jump from wall
        if(Input.GetButtonDown("Jump") && isWallSliding){
            TurnAround(direction);
            Jump();
            jumpTimeCounter -= Time.deltaTime;
            myAnim.SetTrigger("Jump");
        }

        //long jump from wall
        if (Input.GetButton("Jump") && !stoppedJumping && (jumpTimeCounter > 0) && isWallSliding)
        {
            TurnAround(direction);
            Jump();
            jumpTimeCounter -= Time.deltaTime;
            myAnim.SetTrigger("Jump");
        }

        //if player moves down - he's falling
        if (rb2d.velocity.y < 0)
        {
            myAnim.SetBool("Falling", true);
        }

        //if you press the button - player will attack
        if(Time.time >= nextAttackTime && canAttack){
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine("SwordAttackSlash");
                Attack();
                myAnim.SetTrigger("Attack1");
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if(Input.GetButtonDown("Fire2")){
                StartCoroutine("SwordAttackPuncture");
                Attack();
                myAnim.SetTrigger("Attack2");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        CheckIfWallsliding();
        if (!grounded){
            canAttack = false;
        }
        else{
            canAttack = true;
        }

        if(CC.isOpen){
            canWallSlide = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            if(canDash && isMoving && invulnerable == false){
                forceToDash = new Vector2(dashSpeed * speed * direction, 0);
                StartCoroutine("Dashing");
                myAnim.SetTrigger("Dash");
            }
        }
    }
    IEnumerator Dashing(){
        canJump = false;
        canMove = false;
        canDash = false;
        rb2d.AddForce(forceToDash, ForceMode2D.Impulse);
        canAttack = false;
        Physics2D.IgnoreLayerCollision(6, 3);
        invulnerable = true;
        yield return new WaitForSeconds(1.2f);
        canJump = true;
        canMove = true;
        canDash = true;
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
        canAttack = true;
        Physics2D.IgnoreLayerCollision(6, 3, false);
        invulnerable = false;
    }

    //name of this function says for itself
    private void CheckIfWallsliding(){
        if(isTouchingWall && !grounded && rb2d.velocity.y < 0 && canWallSlide){
            isWallSliding = true;
        }
        else{
            isWallSliding = false;
        }
    }

    protected override void HandleMovement()
    {
        base.HandleMovement();
        myAnim.SetFloat("floatSpeed", Mathf.Abs(direction));
        TurnAround(direction);

        //slowing down if we're sliding down the wall
        if(isWallSliding){
            if(rb2d.velocity.y < -wallSlidingSpeed){
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlidingSpeed);
            }
        }
    }

    //detecting enemy in attack range and dealing damage to them

    IEnumerator SwordAttackSlash(){
        canMove = false;
        rb2d.velocity = new Vector2(0,rb2d.velocity.y);
        canJump = false;
        yield return new WaitForSeconds(0.7f);
        canMove = true;
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
        canJump = true;
    }
    
    IEnumerator SwordAttackPuncture(){
        canMove = false;
        rb2d.velocity = new Vector2(0,rb2d.velocity.y);
        canJump = false;
        yield return new WaitForSeconds(0.6f);
        canMove = true;
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
        canJump = true;
    }


    //drawing gizmos for ground- and wall-check
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, radOCircle);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

    private void Flip()
    {
        if(!isWallSliding){
            facingDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    //handling layers in engine
    protected void HandleLayers()
    {
        if (!grounded)
        {
            myAnim.SetLayerWeight(1, 1);
        }
        else
        {
            myAnim.SetLayerWeight(1, 0);
        }
    }
}
