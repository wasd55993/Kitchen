using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PauseUI : MonoBehaviour
{
    //设置界面
    [SerializeField] private SettingsUI uiSettings;

    //按键
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Button Continue;
    [SerializeField] private Button Setting;
    [SerializeField] private Button BackMenu;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        GameManager.Instance.OnGamePause += GameManager_OnGamePause;
        Continue.onClick.AddListener(() =>
        {
            Hide();
            Time.timeScale = 1;
        });
        Setting.onClick.AddListener(() =>
        {
            uiSettings.Show();
        });
        BackMenu.onClick.AddListener(() =>
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
            LoadScene.Load(LoadScene.SceneName.GameMenuScene);
            LoadScene.LoadTargetScene();

            Time.timeScale = 1;
        });
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGamePause -= GameManager_OnGamePause;
    }

    private void GameManager_OnGamePause(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetIsPause())
        {
            Show();
        }
        else
        {
            Hide();
            uiSettings.Hide();
        }
    }

    private void Hide()
    {
        pauseUI.gameObject.SetActive(false);
    }
    private void Show()
    {
        pauseUI.gameObject.SetActive(true);
    }
}
