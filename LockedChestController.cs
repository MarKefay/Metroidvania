using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedChestController : MonoBehaviour
{
    public bool isOpen;
    public Animator myAnim;
    public CorpseController corpseContr;

    public void ChestOpen(){
        if(!isOpen && corpseContr.isOpen){
            isOpen = true;
            Debug.Log("Chest is now open");
            myAnim.SetBool("IsOpen", isOpen);
        }
    }
}

