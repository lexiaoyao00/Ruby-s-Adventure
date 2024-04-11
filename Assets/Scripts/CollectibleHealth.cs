using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    public AudioClip audioClip;
    public int healthValue = 1;

    public GameObject effectParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController ruby = collision.GetComponent<RubyController>();

        if (ruby != null)
        {
            if (ruby.ChangeHealth(healthValue))
            {
                Instantiate(effectParticle, transform.position,Quaternion.identity);
                ruby.PalySound(audioClip);
                Destroy(this.gameObject);
            }

        }

        

    }
}
