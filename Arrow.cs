using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 20;
    public Rigidbody2D rb2d;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        rb2d.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) {
        Character enemy = hitInfo.GetComponent<Player>();
        if(enemy != null){
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
