using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerCombat : MonoBehaviour
{
    private Animator myAnim;
    public int numberOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.9f;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.deltaTime - lastClickedTime > maxComboDelay)
        {
            numberOfClicks = 0;
        }

        if (Input.GetButton("Fire1"))
        {
            lastClickedTime = Time.deltaTime;
            numberOfClicks += 1;
            if (numberOfClicks == 1)
            {
                myAnim.SetBool("Attack1", true);
            }
            else
            {
                myAnim.SetBool("Attack1", false);
                numberOfClicks = 0;
            }
        }
        numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 3);
    }

    private void return1()
    {
        if (numberOfClicks == 2)
        {
            myAnim.SetBool("Attack2", true);
        }
        else
        {
            myAnim.SetBool("Attack2", false);
            myAnim.SetBool("Attack1", false);
        }
    }

    private void return2()
    {
        if (numberOfClicks == 3)
        {
            myAnim.SetBool("Attack3", true);
        }
        else
        {
            myAnim.SetBool("Attack3", true);
            myAnim.SetBool("Attack2", false);
        }
    }

    private void return3()
    {
        myAnim.SetBool("Attack1", false);
        myAnim.SetBool("Attack2", false);
        myAnim.SetBool("Attack3", false);
    }
}
