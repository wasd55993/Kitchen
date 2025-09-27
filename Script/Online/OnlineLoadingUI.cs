using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnlineLoadingUI : MonoBehaviour
{
    [SerializeField] private GameObject onlineLodingVisual;
    [SerializeField] private TextMeshProUGUI loadText;
    private float dotInterval = 0.5f;

    private void Awake()
    {
        Hide();
    }

    /// <summary>
    /// 用于显示加载中的点点点
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            for (int i = 0; i <= 6; i++)
            {
                loadText.text = "正在链接服务器" + new string('.', i);
                yield return new WaitForSeconds(dotInterval);
            }
        }
    }

    public void Show()
    {
        onlineLodingVisual.SetActive(true);
        StartCoroutine(AnimateLoadingText());
    }
    public void Hide()
    {
        StopCoroutine(AnimateLoadingText());
        onlineLodingVisual.SetActive(false);
    }
}
