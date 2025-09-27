using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    //UI控制
    [SerializeField] private GameObject SettingUI;
    [SerializeField] private MusicSettingUI musicSettingUI;
    [SerializeField] private GameButtonSettingUI gameInputButtonSettingUI;

    //按钮
    [SerializeField] private Button MusicSettingButton;
    [SerializeField] private Button GameInputSettingButton;
    [SerializeField] private Button CloseButton;

    private void Awake()
    {
        Hide();

        MusicSettingButton.onClick.AddListener(() =>
        {
            musicSettingUI.Show();
        });
        GameInputSettingButton.onClick.AddListener(() =>
        {
            gameInputButtonSettingUI.Show();
        });
        CloseButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    public void Show()
    {
        SettingUI.SetActive(true);
    }
    public void Hide() 
    {
        SettingUI.SetActive(false);
    }
}
