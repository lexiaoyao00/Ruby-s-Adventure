using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public float displayTime = 4f;
    private float timerDisplay;
    public TextMeshProUGUI dialogText;
    public AudioSource audioSource;
    public AudioClip completeTaskClip;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1;
    }

    // Update is called once per frame
    void Update()
    {
        AutoClose();
        ManualClose();
    }

    public void DisplayDialog()
    {
        GameFrame.instance.hasTask = true;
        timerDisplay = displayTime;
        dialogBox.SetActive (true);
        if (GameFrame.instance.isCompleteTask)
        {
            
            dialogText.text = "Å¶,Î°´óµÄRuby£¬Ð»Ð»Äã£¬ÄãÕæµÄÌ«°ôÁË¡£";
            if (GameFrame.instance.taskHasBeenAccepted)
            {
                audioSource.PlayOneShot(completeTaskClip);
            }
            GameFrame.instance.taskHasBeenAccepted = false;

        }
        else
        {
            if (!GameFrame.instance.taskHasBeenAccepted)
            {
                GameFrame.instance.CreatEnemy();
            }
            GameFrame.instance.taskHasBeenAccepted = true;
        }


    }

    public void CloseDialog()
    {
        timerDisplay = -1;
        if (dialogBox.activeInHierarchy) dialogBox.SetActive(false);
    }

    void AutoClose()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
        }
        else
        {
            CloseDialog();
        }
    }

    void ManualClose()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CloseDialog();
        }

    }
}
