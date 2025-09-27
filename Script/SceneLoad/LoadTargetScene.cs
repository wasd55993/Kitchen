using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTargetScene : MonoBehaviour
{
    private bool isInOnline = false;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;//所有玩家加载游戏场景
    }
    private void Start()
    {
        isInOnline = IsInOnline();
        if (isInOnline)
        {
            LoadScene.Load(LoadScene.SceneName.GameScene);
            PhotonNetwork.LoadLevel(LoadScene.GetTargetSceneNumber());
        }
        else
        {
            LoadScene.LoadTargetScene();
        }
    }

    private bool IsInOnline()
    {
        return PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
    }
}
