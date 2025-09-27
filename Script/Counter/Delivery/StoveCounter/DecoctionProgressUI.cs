using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoctionProgressUI : MonoBehaviour
{
    [SerializeField] private Image progresImage;

    private bool isExtinguish = false;

    /// <summary>
    /// 显示进度条
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏进度条
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 进度条或者警告
    /// </summary>
    public void CookedProgress(float progres)
    {
        if(progresImage.type == Image.Type.Filled) progresImage.fillAmount = progres;
    }

    /*
    IEnumerator Flashing()
    {
        yield return new WaitForSeconds(1f);

        Color tempColor = progresImage.color;
        tempColor.a = isExtinguish ? 1f : 0.5f;

        isExtinguish = !isExtinguish;

        
        progresImage.color = tempColor;
    }*/
}
