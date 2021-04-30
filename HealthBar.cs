using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpBar;

    public void SetMax(int hp){
        hpBar.maxValue = hp;
        hpBar.value = hp;
    }

    public void CalcHealth(int hp){
        hpBar.value = hp;
    }
}
