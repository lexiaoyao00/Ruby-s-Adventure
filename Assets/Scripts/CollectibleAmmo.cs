using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAmmo : MonoBehaviour
{
    public AudioClip audioClip;
    public int ammoValue = 5;

    public GameObject effectParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController ruby = collision.GetComponent<RubyController>();

        if (ruby != null)
        {
            ruby.projectileNum += ammoValue;
            UIBulletIcon.instance.SetValue(ruby.projectileNum);
            Instantiate(effectParticle, transform.position, Quaternion.identity);
            ruby.PalySound(audioClip);
            Destroy(this.gameObject);
            

        }
    }
}
