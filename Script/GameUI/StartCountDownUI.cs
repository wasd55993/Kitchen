using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCountDownUI : MonoBehaviour
{
    private const string IS_SHAKE = "isShake";

    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private TextMeshProUGUI gameTimingUI;
    private Animator animator;
    private int lastNumber = -1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.Instance.OnStartCountDown += GameManaer_OnStartCountDown;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStartCountDown -= GameManaer_OnStartCountDown;
    }

    private void GameManaer_OnStartCountDown(object sender, EventArgs e)
    {
        gameTimingUI.gameObject.SetActive(false);
        countDown.gameObject.SetActive(true);
        
        int nowNumber = Mathf.CeilToInt(GameManager.Instance.GetCountDownToStartTimer());
        countDown.text = nowNumber.ToString();

        if (nowNumber != lastNumber)
        {
            lastNumber = nowNumber;
            animator.SetTrigger(IS_SHAKE);
        }
    }
}
