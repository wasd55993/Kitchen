using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class OnlineManager : MonoBehaviour,IConnectionCallbacks,ILobbyCallbacks
{
    //UI控制
    [SerializeField] private OnlineLoadingUI onlineLoadingUI;
    [SerializeField] private HallUI hallUI;

    //大厅
    TypedLobby lobby;
    public bool IsInLobby { get; private set; } = false;//标记是否进入大厅

    //单例模式，提供全局访问点
    public static OnlineManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        //注册PUN2
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        //注销PUN2
        PhotonNetwork.RemoveCallbackTarget(this);
    }



    /// <summary>
    /// 显示联机加载界面
    /// </summary>
    public void ShowOnlineLoadingUI()
    {
        onlineLoadingUI.Show();
    }
    /// <summary>
    /// 隐藏联机加载页面
    /// </summary>
    public void HideOnlineLoadingUI()
    {
        onlineLoadingUI.Hide();
    }


    /// <summary>
    /// 链接PUN2服务器
    /// </summary>
    public void ConnectPunServer()
    {
        ShowOnlineLoadingUI();
        PhotonNetwork.ConnectUsingSettings();//链接成功执行OnConnectedToMaster()函数
    }

    /// <summary>
    /// 让别的类能够使用配置好的Lobby
    /// </summary>
    /// <returns></returns>
    public TypedLobby GetLobby()
    {
        return lobby;
    }

    #region Pun2接口
    public void OnConnected()
    {
        
    }

    //链接成功后执行的函数
    public void OnConnectedToMaster()
    {
        HideOnlineLoadingUI();

        
        lobby = new TypedLobby("demoLobby", LobbyType.SqlLobby);
        //进入大厅
        PhotonNetwork.JoinLobby(lobby);
    }

    //断开服务器时执行的函数
    public void OnDisconnected(DisconnectCause cause)
    {
        HideOnlineLoadingUI();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    //大厅接口
    //加入大厅成功调用
    public void OnJoinedLobby()
    {
        hallUI.Show();
        Debug.Log("加入大厅成功");
        
        IsInLobby = true;
    }
    //加入大厅失败调用
    public void OnLeftLobby()
    {
        
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        
    }
    #endregion
}
