using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;

public class CreateRoomUI : MonoBehaviourPunCallbacks
{
    //房间ID
    [SerializeField] private TextMeshProUGUI RoomID;

    [SerializeField] private Button confirm;
    [SerializeField] private Button cancel;

    [SerializeField] private GameObject LoadCreateUI;
    private void Awake()
    {
        Hide();

        confirm.onClick.AddListener(() =>
        {
            CreateOnlineRoom();
        });
        cancel.onClick.AddListener(() =>
        {
            Hide();
        });
        RoomID.text = "房间号：" + Random.Range(1, 9999);
    }

    public void CreateOnlineRoom()
    {
        Debug.Log("按下");

        if (!OnlineManager.Instance.IsInLobby)
        {
            Debug.Log("尚未加入大厅");
        }

        LoadCreateUI.SetActive(true);

        //创建房间
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(RoomID.text, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        LoadCreateUI.SetActive(false);
        Hide();
        Debug.Log("创建成功");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        LoadCreateUI.SetActive(false);
        Debug.LogError("房间创建失败，错误码：" + returnCode + "，错误信息：" + message);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
