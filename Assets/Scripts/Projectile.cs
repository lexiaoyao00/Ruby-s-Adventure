using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float liveTime = 3f;

    //子弹受力大小
    public float force = 300f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SelfDestroy();
    }

    public void Launch(Vector2 direction)
    {
        rb.AddForce(direction *  this.force);
    }

    public void Launch(Vector2 direction,float force)
    {
        rb.AddForce(direction * force);
    }



    void SelfDestroy()
    {
        liveTime -= Time.deltaTime;
        if (liveTime <= 0) Destroy(this.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);

        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Fix();
        }

    }
}
