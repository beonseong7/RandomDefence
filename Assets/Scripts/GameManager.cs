using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using DI = System.Diagnostics;
using System;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private string nick_Name;
    public AudioClip[] audioClips;
    public Slider[] sliders;
    public AudioSource audioSource;
    public static GameManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindAnyObjectByType(typeof(GameManager)) as GameManager;
                if (instance == null) Debug.Log("싱글톤 오브젝트 없음");
            }
            return instance;
        }
    }
    public string Nick_Name
    {
        get { return nick_Name; }
        set { nick_Name = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        var processList = DI.Process.GetProcesses();
        foreach (var process in processList)
        {
            Debug.Log(process.ProcessName);
        }
    }
    public void SetData(string Key,string curData)
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { Key, curData } },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, (result) => { }, (error) => print("데이터 저장 실패"));
    }
    public void Change_Audio(string audio)
    {
        this.GetComponent<AudioSource>().Stop();
        if (audio=="main")
        this.GetComponent<AudioSource>().clip = audioClips[1];
        else if(audio=="ingame")
            this.GetComponent<AudioSource>().clip = audioClips[0];
        this.GetComponent<AudioSource>().Play();
    }
    public void OnGUI()
    {
        audioSource.volume = sliders[0].value;
    }
    public void Button_Active(GameObject tmp)
    {
        tmp.SetActive(!tmp.activeSelf);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
