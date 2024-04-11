using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        Patrol,
        Pursuit,
        LostTarget
    }
    private State currentState = State.Patrol;
    private Vector3 originalPos;
    private Vector3 rubyPos;

    public float moveSpeed = 2f;
    public float moveTime = 2f;
    private float timer;
    public bool vertical = false;
    private int direction = 1;

    public int AttackPower = 1;

    private Rigidbody2D rb;
    private Animator animator;

    //»úÆ÷ÈËÊÇ·ñ¹ÊÕÏ
    private bool broken = true;

    public ParticleSystem smokeEffect;

    private AudioSource audioSource;
    public AudioClip fixedClip;
    public AudioClip[] hitSounds;

    public GameObject hitEffectParticle;


    private void Start()
    {
        timer = moveTime;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //animator.SetFloat("MoveX", direction);
        //animator.SetBool("Vertical", vertical);
        PlayMoveAnimation();
        audioSource = GetComponent<AudioSource>();
        originalPos = transform.position;

    }


    private void FixedUpdate()
    {
        if (!broken) return;
        StartCoroutine(FindPlayer());
        StartCoroutine(Chase());

        if (currentState == State.Patrol)
        {
            PatrolMove();
        }
        //print("****Update: currentState: " + currentState);

        //MoveToTarget(new Vector3(0,0,0));
    }

    private void Move()
    {
        Vector2 v = this.transform.position;
        float distance = moveSpeed * Time.deltaTime;
        if (!vertical)
        {
            v.x += distance * direction;
        }
        else
        {
            v.y += distance * direction;
        }
        PlayMoveAnimation();
        rb.MovePosition(v);
    }
    private void MoveToTarget(Vector3 target)
    {
        Vector3 dir = target - this.transform.position;
        dir.z = 0;
        //print("MoveToTarget dir: " + dir);
        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y)+0.2f)
        {
            vertical = false;
            if (dir.x >= 0) direction = 1;
            else direction = -1;
        }
        else
        {
            vertical = true;
            if (dir.y >= 0) direction = 1;
            else direction = -1;
        }
        //print("MoveToTarget:" + " vertical:" + vertical + " direction:" + direction);
        Move();
    }



    void PatrolMove()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            direction *= -1;
            //animator.SetFloat("MoveX", direction);
            //PlayMoveAnimation();
            timer = moveTime;
        }

        Move();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController ruby = collision.gameObject.GetComponent<RubyController>();
        if (ruby != null)
        {
            ruby.ChangeHealth(-AttackPower);
            
        }
    }

    //ÒÆ¶¯¶¯»­×´Ì¬
    void PlayMoveAnimation()
    {
        if (vertical)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }


    public void Fix()
    {
        Instantiate(hitEffectParticle, transform.position, Quaternion.identity);
        broken = false;
        rb.simulated = false;

        smokeEffect.Stop();

        animator.SetTrigger("Fixed");
        EnemyCreator.instance.fixedNum++;

        int randomNum = UnityEngine.Random.Range(0, 2);
        audioSource.Stop();
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(hitSounds[randomNum]);
        Invoke("PlayFixedSound", 1f);
    }

    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(fixedClip);
    }

    bool PosApproximately(Vector3 pos1, Vector3 pos2)
    {
        Vector3 pos = pos1 - pos2;
        if (pos.magnitude <= 0.2f) return true;
        else return false;
    }

    IEnumerator FindPlayer()
    {
            yield return 0;

            Collider2D ruby = Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Character"));
            if (ruby != null)
            {
                currentState = State.Pursuit;
                rubyPos = ruby.transform.position;
                //print(rubyPos);

            }
            else
            {
                if(currentState == State.Pursuit)
                {
                    //print("FindPlayer****" + originalPos + "currentState: " + currentState);
                    currentState = State.LostTarget;
                    //print("change currentState to :" + currentState);
                }
                else if ( PosApproximately(transform.position, originalPos) && currentState == State.LostTarget)
                {
                    //print("FindPlayer****" + originalPos + "currentState: " + currentState);
                    //print("FindPlayer**** LostTarget switch in" + currentState);
                    currentState = State.Patrol;
                    //print("change currentState to :" + currentState);
                }
                else
                {
                //print("FindPlayer**** pos now " + transform.position);
                    //print("FindPlayer****" + originalPos + "currentState: " + currentState);

            }
        }

        
    }

    IEnumerator Chase()
    {
        yield return 0;
        if (currentState == State.Pursuit)
        {
            MoveToTarget(rubyPos);
        }
        else if(currentState == State.LostTarget)
        {
            MoveToTarget(originalPos);
        }
        else
        {
            //print("Pursue" + currentState);
        }

        
    }
    
}
