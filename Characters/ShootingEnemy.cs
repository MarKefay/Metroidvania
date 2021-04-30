using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class ShootingEnemy : Character
{
    public Transform target;
    public Transform fieldOfView;
    public Transform groundDetection;
    public Transform enemyDetection;
    public Transform enemyDetectionFromBack;
    RaycastHit2D findCharacters;
    RaycastHit2D detectEnemy;
    RaycastHit2D detectEnemyFromBack;
    RaycastHit2D WallInfo;

    public Transform firepoint;
    public GameObject arrowPrefab;
    public float rayDistance;
    public float chaseSpeed;
    public float nextWaypointDistance = 3;
    public float FOVDistance = 2f;
    public float rangeOfView;
    public float bckDetection;

    public int maxEnemyHealth = 100;
    int currentWayPoint = 0;

    protected bool readyToAttack;
    public bool enemyFound = false;
    public bool enemyInRange = false;
    public bool enemyIsDead = false;
    private bool movingRight = true;
    bool reachedEndOfPath = false;


    public override void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        currentHealth = maxEnemyHealth;
        readyToAttack = true;
        facingRight = false;
        speed = chaseSpeed;
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        myAnim.SetFloat("Speed", chaseSpeed);
        if(movingRight){
            findCharacters = Physics2D.Raycast(fieldOfView.position, Vector2.right, FOVDistance, enemyLayers);
            WallInfo = Physics2D.Raycast(groundDetection.position, Vector2.right, rayDistance);
            detectEnemy = Physics2D.Raycast(enemyDetection.position, Vector2.right, rangeOfView, enemyLayers);
            detectEnemyFromBack = Physics2D.Raycast(enemyDetectionFromBack.position, Vector2.left, bckDetection, enemyLayers);
        }
        else{
            findCharacters = Physics2D.Raycast(fieldOfView.position, Vector2.left, FOVDistance, enemyLayers);
            WallInfo = Physics2D.Raycast(groundDetection.position, Vector2.left, rayDistance);
            detectEnemy = Physics2D.Raycast(enemyDetection.position, Vector2.left, rangeOfView, enemyLayers);
            detectEnemyFromBack = Physics2D.Raycast(enemyDetectionFromBack.position, Vector2.right, bckDetection, enemyLayers);
        }
        RaycastHit2D GroundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);
        if(findCharacters.collider == true){
            enemyInRange = true;
        }
        else {
            enemyInRange = false;
        }
        //else{
        if(detectEnemy.collider == true){
            enemyFound = true;
        }
        else{
            enemyFound = false;
        }
        if(detectEnemyFromBack.collider == true){
            if(movingRight || !movingRight){
                movingRight = !movingRight;
                transform.Rotate(0f, -180f, 0f);
            }
        }
        if(enemyFound == true){
            transform.position = Vector2.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
        }
        else{
        transform.Translate(Vector2.right * chaseSpeed * Time.deltaTime);
        if(GroundInfo.collider == false || WallInfo.collider == true){
            if(movingRight || !movingRight){
                movingRight = !movingRight;
                transform.Rotate(0f, -180f, 0f);
            }
        }
        }
        if(enemyInRange){
            chaseSpeed = 0f;
            if(Time.time >= nextAttackTime && readyToAttack){
            StartCoroutine("EnemyAttack");
            Shoot();
            myAnim.SetTrigger("Attack1");
            nextAttackTime = Time.time + 1f / attackRate;
            }
        }else{
        chaseSpeed = 0.5f;
        }
        if(currentHealth == 0f){
            enemyIsDead = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawLine(groundDetection.position, new Vector3(groundDetection.position.x, groundDetection.position.y - rayDistance, groundDetection.position.z));
        Gizmos.DrawLine(groundDetection.position, new Vector3(groundDetection.position.x + rayDistance, groundDetection.position.y, groundDetection.position.z));
        Gizmos.DrawLine(fieldOfView.position, new Vector3(fieldOfView.position.x + FOVDistance, fieldOfView.position.y, fieldOfView.position.z));
        Gizmos.DrawLine(enemyDetection.position, new Vector3(enemyDetection.position.x + rangeOfView, enemyDetection.position.y, enemyDetection.position.z));
        Gizmos.DrawLine(enemyDetectionFromBack.position, new Vector3(enemyDetectionFromBack.position.x - bckDetection, enemyDetectionFromBack.position.y, enemyDetectionFromBack.position.z));
    }

    IEnumerator EnemyAttack(){
        chaseSpeed = 0f;
        yield return new WaitForSeconds(0.7f);
        chaseSpeed = 0.5f;
    }

    void Shoot(){
        Instantiate(arrowPrefab, firepoint.position, firepoint.rotation);
    }
}
