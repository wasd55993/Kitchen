using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private TextMeshProUGUI gameTimingUI;
    [SerializeField] private Image gameTimingImage;
    [SerializeField] private Image background;
    [SerializeField] private Image fill;

    private void Awake()
    {
        background.gameObject.SetActive(false);
        fill.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnGameTiming += GameManaer_OnGameTiming;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStartCountDown -= GameManaer_OnGameTiming;
    }

    private void GameManaer_OnGameTiming(object sender, EventArgs e)
    {
        countDown.gameObject.SetActive(false);
        gameTimingUI.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        fill.gameObject.SetActive(true);
        gameTimingUI.text = Mathf.CeilToInt(GameManager.Instance.GetGamePlayingTimer()).ToString();
        gameTimingImage.fillAmount = GameManager.Instance.GetGamePlayingTimer() / GameManager.Instance.GetMaxGamePlaying();
    }
}
