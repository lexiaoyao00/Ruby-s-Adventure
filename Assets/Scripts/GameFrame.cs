using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFrame : MonoBehaviour
{
    enum MScence {
        Scence_01,
        Scence_02
    }
    public static GameFrame instance {  get; private set; }


    public GameObject ruby;
    public bool taskHasBeenAccepted = false;
    public bool hasTask=false;
    public bool isCompleteTask = false;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        //CreatEnemy();
    }

    private void Update()
    {
        if (hasTask)
        {
            if (EnemyCreator.instance.TaskComplete())
            {
                isCompleteTask = true;
            }
        }
    }

    public void CreatEnemy()
    {
        //print("CreatEnemy called");
        EnemyCreator.instance.CreatEnemys();
    }

    public void TaskFailed()
    {
        //TODO
        print("task failed");
    }


}
