using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //音乐音量数据键值
    private static string MUSICMANAGER_VOLUME = "MusicValume";
    //按键数据键值
    private static string KEYDATA = "KeyData";

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// 保存音乐音量数据
    /// </summary>
    /// <param name="volume"></param>
    public void SaveMusicData(float volume)
    {
        // 限制音量值在0-1范围内
        float clampedVolume = Mathf.Clamp01(volume);

        try
        {
            PlayerPrefs.SetFloat(MUSICMANAGER_VOLUME, clampedVolume);
            PlayerPrefs.Save(); // 立即保存，防止数据丢失
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save music volume: {e.Message}");
        }
    }

    /// <summary>
    /// 加载音乐音量数据
    /// </summary>
    /// <returns></returns>
    public float LoadMusicData()
    {
        try
        {
            // 检查键是否存在，避免使用默认值时的歧义
            if (PlayerPrefs.HasKey(MUSICMANAGER_VOLUME))
            {
                return Mathf.Clamp01(PlayerPrefs.GetFloat(MUSICMANAGER_VOLUME));
            }
            return 1f;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save music volume: {e.Message}");
            return 1f;
        }
    }

    /// <summary>
    /// 保存按键数据
    /// </summary>
    /// <param name="keyData"></param>
    public void SaveKeyData(string keyData)
    {
        try 
        {
            PlayerPrefs.SetString(KEYDATA, keyData);
            PlayerPrefs.Save();
        }
        catch (System.Exception e) 
        {
            Debug.LogError($"Failed to save music volume: {e.Message}");
        }
    }
    /// <summary>
    /// 读取按键数据
    /// </summary>
    public string LoadKeyData()
    {
        try
        {
            if (PlayerPrefs.HasKey(KEYDATA))
            {
                return PlayerPrefs.GetString(KEYDATA);
            }
            return null;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save music volume: {e.Message}");
            return null;
        }
    }
}
