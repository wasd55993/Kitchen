using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InciseProgressUI : MonoBehaviour
{
    [SerializeField] private Image progresImage;

    /// <summary>
    /// 显示进度条
    /// </summary>
    public void Show() { gameObject.SetActive(true); }
    
    /// <summary>
    /// 隐藏进度条
    /// </summary>
    public void Hide() { gameObject.SetActive(false); }

    /// <summary>
    /// 更新进度条
    /// </summary>
    public void UpdateProgress(float progress)
    {
        progresImage.fillAmount = progress;

        if (progress == 1)
        {
            StartCoroutine(HideProgressUI());
        }
    }

    /// <summary>
    /// 协程控制进度条的关闭
    /// </summary>
    /// <returns></returns>
    IEnumerator HideProgressUI()
    {
        yield return new WaitForSeconds(0.5f);

        Hide();
    }
}
