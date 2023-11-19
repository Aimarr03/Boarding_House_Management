using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<CustomAudio> SFXList;
    public List<CustomAudio> BGMList;

    public AudioSource SFX_Manager;
    public AudioSource BGM_Manager;
    public void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is one more Audio Manager");
            Destroy(instance.gameObject);
        }
        instance = this; 
        DontDestroyOnLoad(gameObject);
    }
    public enum AudioType
    {
        Buy,
        Repairing,
        Cleaning,
        Click,
        Dialogue,
        Building_Menu,
        Build,
        NotEnoughMoney,
        GameOver,
        Pay,
        Win,
        BGM_GameOver,
        BGM_MainMenu,
        BGM_Gameplay
    }
    public void PlaySFX(AudioType type)
    {
        SFX_Manager.PlayOneShot(GetSFX(type));
    }
    public void PlaySFX(int index)
    {
        SFX_Manager.PlayOneShot(SFXList[index].audio);
    }
    public void PlayMusic(AudioType type)
    {
        if(BGM_Manager.clip == GetBGM(type))
        {
            return;
        }
        BGM_Manager.Stop();
        BGM_Manager.clip = GetBGM(type);
        BGM_Manager.Play();
    }
    public AudioClip GetSFX(AudioType type)
    {
        foreach(CustomAudio customAudio in SFXList)
        {
            if(type == customAudio.type)
            {
                return customAudio.audio;
            }
        }
        return null;
    }
    public AudioClip GetBGM(AudioType type)
    {
        foreach (CustomAudio customAudio in BGMList)
        {
            if (type == customAudio.type)
            {
                return customAudio.audio;
            }
        }
        return null;
    }
    [System.Serializable]
    public class CustomAudio
    {
        public AudioType type;
        public AudioClip audio;
    }
}
