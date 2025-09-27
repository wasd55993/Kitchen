using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerObjectSOList")]
public class PlayerObjectSOList : ScriptableObject
{
    public List<PlayerObjectSO> playerObjectList;
}
