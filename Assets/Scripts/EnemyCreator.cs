using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public static EnemyCreator instance {  get; private set; }

    public float width = 15;
    public float height = 10;
    public int enemyNum = 4;
    public GameObject enemyPrefab;
    public GameObject enemyParent;

    public int fixedNum = 0;


    private void Awake()
    {
        instance = this;
    }

    public void CreatEnemys()
    {
        //print("EnemyCreator.CreatEnemys called ");
        for (int i = 0; i < enemyNum; i++)
        {
            float posx = Random.Range(-width, width);
            float posy = Random.Range(-height, height);
            Vector2 randPos = new Vector2(posx, posy);
            //print(randPos);
            Instantiate(enemyPrefab, randPos, Quaternion.identity, enemyParent.transform);
        }
    }

    public bool TaskComplete()
    {
         return (fixedNum >= enemyNum) ? true : false;

    }
}
