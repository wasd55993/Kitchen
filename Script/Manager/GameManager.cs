using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }

    public PlayerControl player;

    private GameState gameState;

    [Header("计时器")]
    private float waitingToStartTimer = 2f;
    private float countDownToStartTimer = 3f;
    private float maxGamePlaying = 45f;
    private float gamePlayingTimer;//游戏时间

    //事件
    public event EventHandler OnStartCountDown;//游戏开始倒计时
    public event EventHandler OnGameTiming;//游戏进行时
    public event EventHandler OnGameOver;//游戏结束
    public event EventHandler OnGamePause;//游戏暂停

    //游戏状态
    private bool isPause = false;

    //单例模式，提供访问点
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        gamePlayingTimer = maxGamePlaying;
        TurnToWaitingToStart();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseEvent += GameInput_OnPauseEvent;
    }
    private void OnDisable()
    {
        GameInput.Instance.OnPauseEvent -= GameInput_OnPauseEvent;
    }

    private void Update()
    {
        switch (gameState) 
        {
            case GameState.WaitingToStart:
                PlayerDisable();
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0)
                {
                    TurnToCountDownToStart();
                }
                break;
            case GameState.CountDownToStart:
                PlayerDisable();
                countDownToStartTimer -= Time.deltaTime;
                OnStartCountDown?.Invoke(this, EventArgs.Empty);
                if (countDownToStartTimer <= 0)
                {
                    TurnToGamePlaying();
                }
                break;
            case GameState.GamePlaying:
                PlayerEnable();
                gamePlayingTimer -= Time.deltaTime;
                OnGameTiming?.Invoke(this, EventArgs.Empty);
                if (gamePlayingTimer <= 0)
                {
                    TurnToGameOver();
                }
                break;
            case GameState.GameOver:
                PlayerDisable();
                break;
        }
    }

    /// <summary>
    /// 控制玩家启用（可以移动）
    /// </summary>
    private void PlayerEnable()
    {
        player.enabled = true;
    }
    /// <summary>
    /// 控制玩家禁用（不能移动）
    /// </summary>
    private void PlayerDisable() 
    {
        player.enabled = false;
    }

    //状态转换
    private void TurnToWaitingToStart()
    {
        gameState = GameState.WaitingToStart;
    }
    private void TurnToCountDownToStart()
    {
        gameState = GameState.CountDownToStart;
    }
    private void TurnToGamePlaying()
    {
        gameState = GameState.GamePlaying;
        OrderManager.Instance.StartCreatOrder();
    }
    private void TurnToGameOver() 
    {
        gameState = GameState.GameOver;
        PlayerDisable();
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    //获取当前状态
    public GameState GetGameState()
    {
        return gameState;
    }

    //获取计时时间
    public float GetMaxGamePlaying()
    {
        return maxGamePlaying;
    }
    public float GetCountDownToStartTimer()
    {
        return countDownToStartTimer;
    }
    public float GetGamePlayingTimer()
    {
        return gamePlayingTimer;
    }

    public bool GetIsPause()
    {
        return isPause;
    }
    
    //事件响应
    private void GameInput_OnPauseEvent(object sender, EventArgs e)
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0; 
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;
            OnGamePause?.Invoke(this, EventArgs.Empty);
        }
    }
}
