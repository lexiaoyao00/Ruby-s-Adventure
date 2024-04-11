using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance {  get; private set; }

    public Image mask;
    float originalSize;

    private void Awake()
    {
        instance = this;
        originalSize = mask.rectTransform.rect.width;
    }

    //…Ë÷√—™ÃıUIœ‘ æ
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * fillPercent);
    }
}
