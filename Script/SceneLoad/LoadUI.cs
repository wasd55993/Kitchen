using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadText;
    private float dotInterval = 0.5f;

    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            for (int i = 0; i <= 6; i++)
            {
                loadText.text = "加载中" + new string('.', i);
                yield return new WaitForSeconds(dotInterval);
            }
        }
    }

    // 在Start或Enable中启动协程
    void Start()
    {
        StartCoroutine(AnimateLoadingText());
    }
}
