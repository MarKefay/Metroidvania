using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class FlyingEnemy : Character
{
    public Transform target;
    public Transform fieldOfView;
    public Transform groundDetection;
    RaycastHit2D WallInfo;
    RaycastHit2D GroundInfo;
    RaycastHit2D findCharactersRight;
    RaycastHit2D findCharactersLeft;
    Seeker seeker;
    Path path;

    public float chaseSpeed = 200f;
    public float nextWaypointDistance = 3;
    public float FOVDistance = 2f;
    public float FOVRangeDistance = 3f;
    public float rayDistance;

    public int maxEnemyHealth = 100;
    int currentWayPoint = 0;

    protected bool readyToAttack;
    public bool enemyFound = false;
    private bool movingRight = true;
    bool reachedEndOfPath = false;



    public override void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        currentHealth = maxEnemyHealth;
        readyToAttack = true;
        facingRight = false;
        speed = chaseSpeed;
        
    }

    void UpdatePath()   {
        if (seeker.IsDone()){
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path a){
        if(!a.error){
            path = a;
            currentWayPoint = 0;
        }
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //enemy will attack player if it got close to him
        if(Time.time >= nextAttackTime && reachedEndOfPath == true && readyToAttack == true){
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
        findCharactersRight = Physics2D.Raycast(fieldOfView.position, Vector2.right, FOVRangeDistance, enemyLayers);
        findCharactersLeft = Physics2D.Raycast(fieldOfView.position, Vector2.left, FOVRangeDistance, enemyLayers);
        

        if(Vector3.Distance(target.position, transform.position) <= FOVDistance || findCharactersRight.collider == true || findCharactersLeft.collider == true)
        {
        //go to player
            enemyFound = true;
        }else{
            enemyFound = false;
        }
    }
    
    public override void FixedUpdate()
    {
        if (path == null){
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }//checking if enemy got close to a target

        //moving enemy towards target
        if(enemyFound == true){
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb2d.position).normalized;
        Vector2 force = direction * chaseSpeed * Time.deltaTime;
        rb2d.AddForce(force);
        }
        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance){
            currentWayPoint++;
        }

        //turning enemy right or left
        if(rb2d.velocity.x <= -0.01f){
            transform.localScale = new Vector3(-1f, 1f, 1f);
            facingRight = false;
        }else if(rb2d.velocity.x >= 0.01f){
            transform.localScale = new Vector3(1f, 1f, 1f);
            facingRight = true;
        }
    }
    

    void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(fieldOfView.position, FOVDistance);
        Gizmos.DrawLine(fieldOfView.position, new Vector3(fieldOfView.position.x + FOVRangeDistance, fieldOfView.position.y, fieldOfView.position.z));
        Gizmos.DrawLine(fieldOfView.position, new Vector3(fieldOfView.position.x - FOVRangeDistance, fieldOfView.position.y, fieldOfView.position.z));
    }
}
