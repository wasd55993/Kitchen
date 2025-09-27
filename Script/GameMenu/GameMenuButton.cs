using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameMenuButton : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button onlineButton;

    private void Start()
    {
        startButton.onClick.AddListener(()=>
        {
            LoadScene.Load(LoadScene.SceneName.GameScene);//设置目标场景
            LoadScene.LoadLoadingScene();
        });
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        onlineButton.onClick.AddListener(() =>
        {
            OnlineManager.Instance.ConnectPunServer();
        });
    }
}
