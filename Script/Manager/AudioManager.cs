using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class AudioType 
    {
        public AudioSource Source;  //声音源
        public AudioClip Clip;  //声音片段
        public string  Name;  //音乐名

        [Range(0f,1f)]   //使得音量大小是以条的形式进行调节
        public float Volume;  //音量大小
        [Range(0.1f,5f)]  //同理 条进行调节
        public float Pitch;  //音高
        public bool Loop;  //是否循环播放

    }
    public static AudioManager instance;
    public AudioType[] audioTypes;
    void Start()
    {
        //初始话音频
        foreach(AudioType audioType in audioTypes)
        {
            //添加音频源
            audioType.Source = gameObject.AddComponent<AudioSource>();
            audioType.Source.clip = audioType.Clip;
            audioType.Source.volume = audioType.Volume;
            audioType.Source.pitch = audioType.Pitch;
            audioType.Source.loop = audioType.Loop;
        }
        //PlaySouce("Land");
    }
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySouce(string name)
    {
        foreach(AudioType audioType in audioTypes)
        {
            if(audioType.Name == name)
            {
                audioType.Source.Play();
            }
        
        else
        {
            Debug.LogWarning("音频名字错误");
        }
        }
    }
    public void StopSource(string name)
    {
        foreach(AudioType audioType in audioTypes)
        {
            if(audioType.Name == name)
            {
                audioType.Source.Stop();
            }
        
        else
        {
            Debug.LogWarning("音频名字错误");
        }
        }
    }
    public void PauseSource(string name)
    {
        foreach(AudioType audioType in audioTypes)
        {
            if(audioType.Name == name)
            {
                audioType.Source.Pause();
            }
            else
            {
                Debug.LogWarning("音频名字错误");
            }
        }
    }

    
}
