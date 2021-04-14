using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public bool isOpen;
    public Animator myAnim;

    public void ChestOpen(){
        if(!isOpen){
            isOpen = true;
            Debug.Log("Chest is now open");
            myAnim.SetBool("IsOpen", isOpen);
        }
    }
}
