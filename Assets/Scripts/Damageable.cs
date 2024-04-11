using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("OnTriggerStay2D called");
        RubyController ruby = collision.GetComponent<RubyController>();

        if (ruby != null)
        {
            //print("OnTriggerStay2D called");
            ruby.ChangeHealth(-damage);
        }
    }
}
