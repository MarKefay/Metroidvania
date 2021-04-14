using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseController : MonoBehaviour
{
    public bool isOpen;
    public Animator myAnim;

    public void ChestOpen(){
        if(!isOpen){
            isOpen = true;
            Debug.Log("Corpse is now empty");
            myAnim.SetBool("isOpen", isOpen);
        }
    }
}
