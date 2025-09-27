using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerObjectSOList playerObjectSOList;
    [SerializeField] private List<Transform> createPoint;
    private bool isInOnline = false;

    private void Awake()
    {
        isInOnline = IsInOnline();
    }

    private void Start()
    {
        CreatePlayerObject();
    }

    public void CreatePlayerObject()
    {
        int index = Random.Range(0, createPoint.Count);

        if (isInOnline)
        {
            Debug.Log($"当前房间玩家数: {PhotonNetwork.CurrentRoom.PlayerCount}");
            GameObject obj =PhotonNetwork.Instantiate(
                playerObjectSOList.playerObjectList[index].GetPlayerPreName(), 
                createPoint[index].position, 
                Quaternion.identity);
            PlayerControl player = obj.GetComponent<PlayerControl>();
            
            player.SetGameInput();//设置控制系统
            GameManager.Instance.player = player;
        }
        else
        {
            GameObject obj = GameObject.Instantiate(playerObjectSOList.playerObjectList[3].playerPre);
            PlayerControl player = obj.GetComponent<PlayerControl>();
            player.SetGameInput();//设置控制系统
            GameManager.Instance.player = player;
        }
    }

    //是否联机状态
    private bool IsInOnline()
    {
        return PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
    }
}
