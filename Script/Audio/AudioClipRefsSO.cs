using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioAssets/AudioClipRefsSO")]
public class AudioClipRefsSO : ScriptableObject
{
    [Header("切菜")]
    public AudioClip[] chop;
    [Header("上错菜")]
    public AudioClip[] deliveryFail;
    [Header("上对菜")]
    public AudioClip[] deliveryTrue;
    [Header("脚步")]
    public AudioClip[] footStep;
    [Header("放置物品")]
    public AudioClip[] objectDrop;
    [Header("拿取物品")]
    public AudioClip[] objectGet;
    [Header("煎肉")]
    public AudioClip[] stoveSizzle;
    [Header("扔掉")]
    public AudioClip[] trash;
    [Header("煎过头")]
    public AudioClip[] warning;
}
