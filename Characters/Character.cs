using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]

public abstract class Character : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] protected float speed = 2.0f;
    [SerializeField] protected float direction;
    protected bool facingRight = true;
    protected bool canMove = true;

    [Header ("Jump Variables")]
    public float jumpForce;
    public float jumpTime;
    protected float jumpTimeCounter;
    protected bool stoppedJumping;
    protected bool canJump = true;

    [Header("Ground Details")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float radOCircle;
    [SerializeField] protected LayerMask whatIsGround;
    public bool grounded;

    [Header ("Attack Variables")]
    public static bool canAttack = false;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    protected float nextAttackTime = 0f;
    public int attackDamage = 40;

    [Header ("Character Stats")]
    protected int currentHealth;
    public bool invulnerable = false;

    [Header("Components")]
    protected Rigidbody2D rb2d;
    protected Animator myAnim;

    
    public float wallHopForce;
    public float wallJumpForce;
    protected int facingDirection = 1;
    protected bool isTouchingWall;
    protected bool isWallSliding;
    public float wallSlidingSpeed = 0.1f;

    public Vector2 wallhopDirection;
    public Vector2 wallJumpDirection;

    [Header("Dash Variables")]
    protected bool canDash = true;
    
    public float dashSpeed;
    protected Vector2 forceToDash;



    public PauseMenu paused;

    #region monos
    public virtual void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

    }
    public virtual void Update() {
        
        grounded = Physics2D.OverlapCircle(groundCheck.position, radOCircle, whatIsGround); //checking if character staying on ground
        //HandleJumping();
        if(paused.gamePaused){
            canAttack = false;
        }
        else{
            canAttack = true;
        }
    }
    public virtual void FixedUpdate() {
        HandleMovement();
    }
    #endregion

    #region mechanics
    protected void Move(){
        //defining movement
        if(canMove){
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
        }
    }

    protected void Jump(){
        //defining jumping
        if(canJump){
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            //Jump();
        }
        else if(isWallSliding && direction == 0 && canJump){
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallhopDirection.x * -facingDirection, wallHopForce * wallhopDirection.y);
            rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        else if ((isWallSliding || isTouchingWall) && direction != 0 && canJump){
            isWallSliding = false;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * direction, wallJumpForce * wallJumpDirection.y);
            rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    protected void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies){
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Character>().TakeDamage(attackDamage);
        }
    }

    //drawing gizmo for the attackpoint
    void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
    }

    //taking damage from enemies
    public void TakeDamage(int damage){
        if (invulnerable == false){
        currentHealth -= damage;
        myAnim.SetTrigger("Hurt");
        StartCoroutine("GetInvulnerable");  //getting invulnerable after we're hit
        if (currentHealth <=0){
            Die();
        }
        }
    }

    //timer for being invulnerable
    IEnumerator GetInvulnerable(){
        canMove = false;
        rb2d.velocity = new Vector2(0,rb2d.velocity.y);
        canJump = false;
        invulnerable = true;
        yield return new WaitForSeconds(.5f);
        canMove = true;
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
        canJump = true;
        invulnerable = false;
    }
    
    //says for itself
    void Die(){
        myAnim.SetBool("IsDead", true);
        Physics2D.IgnoreLayerCollision(3, 6);
        this.enabled = false;
    }
    #endregion

    #region subMechanics
    protected virtual void HandleMovement(){
        //handling movement
        Move();
    }
    
    protected virtual void HandleJumping(){
        
        Jump();
    }

    protected virtual void TurnAround(float horizontal){
        //func for turning left/right
        if  (horizontal < 0 && facingRight || horizontal > 0 && !facingRight) 
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }
    #endregion
}
