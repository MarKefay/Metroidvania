using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public bool isOpen;
    public bool canOpen;
    public Animator myAnim;
    public LockedChestController chestContr;

    public void DoorOpen(){
        if(!isOpen && chestContr.isOpen){
            isOpen = true;
            Debug.Log("Door is now open");
            myAnim.SetTrigger("interacting");
            myAnim.SetBool("isOpen", isOpen);
        }
        if(isOpen){
        Physics2D.IgnoreLayerCollision(6, 10);
        }
    }
}
