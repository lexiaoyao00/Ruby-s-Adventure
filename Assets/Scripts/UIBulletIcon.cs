using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBulletIcon : MonoBehaviour
{
    public static UIBulletIcon instance {  get; private set; }
    public TextMeshProUGUI numText;
    private Color defaultColor = Color.black;
    private void Awake()
    {
        instance = this;
        SetValue(0);
    }

    //�����ӵ�����
    public void SetValue(int bulletNum)
    {
        if(bulletNum <= 0)
        {
            bulletNum = 0;
            bulletNotEnough();
        }
        else
        {
            bulletEnough();
        }
        numText.text = bulletNum.ToString();
        
    }

    //�ӵ�����
    public void bulletNotEnough()
    {
        numText.color = Color.red;
    }

    public void bulletEnough()
    {
        print("bulletEnough:" + defaultColor);
        numText.color = defaultColor;
    }
}
