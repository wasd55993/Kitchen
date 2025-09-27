using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HallUI : MonoBehaviour
{
    [SerializeField] private GameObject hallVisual;
    [SerializeField] private GameObject loadRefreshRoom;
    [SerializeField] private GameObject loadCreateRoom;

    [SerializeField] private Button closeButton;//关闭大厅UI，即退出联机模式

    private void Awake()
    {
        Hide();

        closeButton.onClick.AddListener(() =>
        {
            DisconnectOnline();
        });
    }

    private void DisconnectOnline()
    {
        PhotonNetwork.Disconnect();
        Hide();
        loadRefreshRoom.SetActive(false);
        loadCreateRoom.SetActive(false);
    }

    public void Show()
    {
        hallVisual.SetActive(true);

    }
    public void Hide()
    {
        hallVisual.SetActive(false);
    }
}
