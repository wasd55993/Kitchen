using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerControl player;
    private float stepSoundRate = 0.15f;
    private float stepSoundTimer = 0;

    private void Start()
    {
        player = GetComponent<PlayerControl>();
    }

    private void Update()
    {
        stepSoundTimer += Time.deltaTime;
        if (stepSoundTimer >= stepSoundRate)
        {
            stepSoundTimer = 0;
            if (player.IsMove)
            {
                Debug.Log("行走");
                SoundManager.Instance.PlayerSound();
            }
        }
    }
}
