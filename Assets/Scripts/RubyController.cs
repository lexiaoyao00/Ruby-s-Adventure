using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class RubyController : MonoBehaviour 
{
    private Rigidbody2D rb;

    public float moveSpeed = 3f;

    public int maxHealth = 5;
    private int currentHealth;


    public float timeInvincible = 1f;
    private bool isInvincible;
    private float invicibleTimer;

    private Vector2 lookDir = new Vector2 (1, 0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;
    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkClip;
        
    public int projectileNum = 0;

    private Vector3 respawnPostion = new Vector3(2.5f, -25, 0);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = respawnPostion;
        currentHealth = maxHealth;
        invicibleTimer = timeInvincible;
        isInvincible = false;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();

        UIBulletIcon.instance.SetValue(projectileNum);
    }
   

    private void FixedUpdate()
    {
        MoveControl();
    }

    private void Update()
    {
        InvincibleTimeDetection();
        
        if (Input.GetKeyDown(KeyCode.J  ))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DetectNPC();
        }
    }

    void InvincibleTimeDetection()
    {
        if (isInvincible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer <= 0 ) 
            {
                isInvincible = false;   
            }
        }
    }



    void LookDirection(Vector2 move )
    {
        //玩家输入轴向不为零
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            lookDir.Set(move.x, move.y);
            lookDir.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.Play();

            }
            walkAudioSource.clip = walkClip;
        }
        else
        {
            walkAudioSource.Stop();
        }
    }

    void PlayMoveAnimation(Vector2 move)
    {
        animator.SetFloat("Look X",lookDir.x);
        animator.SetFloat("Look Y",lookDir.y);
        animator.SetFloat("Speed", move.magnitude);
    }

    void MoveControl()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);

        LookDirection(move);
        PlayMoveAnimation(move);

        Vector2 p = transform.position;
        p += move * moveSpeed * Time.deltaTime;

        rb.MovePosition(p); 
    }

    public bool ChangeHealth(int amount)
    {

        if (amount < 0)
        {
            if (isInvincible)
            {
                return false;
            }

            isInvincible = true;
            invicibleTimer = timeInvincible;
            PalySound(playerHit);
            animator.SetTrigger("Hit");
        }

        int temp = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth+amount, 0, maxHealth);
        //print(currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            Respawn();
        }

        if (temp == currentHealth)
        {
            return false;
        }

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        return true;
    }

    void Launch()
    {
        if (!GameFrame.instance.hasTask)
        {
            return;
        }

        if (projectileNum > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDir);

            animator.SetTrigger("Launch");
            PalySound(attackSoundClip);
            projectileNum -= 1;
            UIBulletIcon.instance.SetValue(projectileNum);
        }
        else
        {

            if (GameFrame.instance.isCompleteTask)
            {
                GameFrame.instance.TaskFailed();
            }
        }


    }

    void DetectNPC()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position+Vector2.up*0.2f,
            lookDir,
            1.5f,
            LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
            if (npcDialog != null)
            {
                npcDialog.DisplayDialog();
            }
        }
    }

    public void PalySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPostion;
    }
}
