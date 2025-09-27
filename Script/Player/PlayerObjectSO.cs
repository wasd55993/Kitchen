using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerObjectSO")]
public class PlayerObjectSO : ScriptableObject
{
    public GameObject playerPre;
    public bool isCreate;

    public string GetPlayerPreName()
    {
        return playerPre.name;
    }
}
