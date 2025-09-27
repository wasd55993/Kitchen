using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI successCount;
    [SerializeField] private TextMeshProUGUI faileCount;

    private void Awake()
    {
        gameOverUI.SetActive(false);
    }

    /// <summary>
    /// 设置制作的菜品数量,并显示是否上菜成功
    /// </summary>
    /// <param name="count1">上菜成功数量</param>
    /// <param name="count2">上菜失败数量</param>
    public void SetRecipeCount(int count1,int count2)
    {
        gameOverUI.SetActive(true);
        successCount.text = count1.ToString();
        faileCount.text = count2.ToString();
    }
}
