using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private const string MUSICVOLUME = "MusicVolume";

    //更新频率的阈值,防止更新频率太快
    private const float VALUE_CHANGE_THRESHOLD = 0.01f;
    private float lastSavedValue;//未更新前的音量，用于阈值判断

    private void Start()
    {
        InitializeValue();

        musicSlider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        musicSlider.onValueChanged.RemoveListener(OnValueChanged);
    }

    /// <summary>
    /// 初始化，读取保存的音量数据
    /// </summary>
    private void InitializeValue()
    {
        float volume = DataManager.Instance.LoadMusicData();
        musicSlider.value = volume;//滑块更新
        lastSavedValue = volume;
        UpdateAudioMixer(volume);
    }

    /// <summary>
    /// 设置混音器音量
    /// </summary>
    /// <param name="volume"></param>
    private void UpdateAudioMixer(float volume)
    {
        float value = Mathf.Lerp(-80f,20f,volume);
        audioMixer.SetFloat(MUSICVOLUME, value);
    }

    /// <summary>
    /// 滑动条改变时，保存新的音量数据
    /// </summary>
    /// <param name="arg0"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnValueChanged(float volume)
    {
        if (Mathf.Abs(volume - lastSavedValue) > VALUE_CHANGE_THRESHOLD)
        {
            DataManager.Instance.SaveMusicData(volume);
            lastSavedValue = volume;
        }

        UpdateAudioMixer(volume);
    }
}
