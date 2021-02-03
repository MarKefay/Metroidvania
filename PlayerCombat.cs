using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerCombat : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator myAnim;

    private void Start()
    {
        //define the game objs find on the player
        rb2d = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        myAnim.SetTrigger("Attack");
    }
}
