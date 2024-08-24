using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Charac_Data charac_Data=new Charac_Data();
    public static GameManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindAnyObjectByType(typeof(GameManager)) as GameManager;
                if (instance == null) Debug.Log("ΩÃ±€≈Ê ø¿∫Í¡ß∆Æ æ¯¿Ω");
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
        /*foreach (var process in processList)
        {
            Debug.Log(process.ProcessName);
        }*/
        
    }
    private void Start()
    {
        charac_Data.Set_Status();
    }
    public void Change_Audio(string audio)
    {
        this.GetComponent<AudioSource>().Stop();
        if (audio=="main")
        this.GetComponent<AudioSource>().clip = audioClips[1];
        else if(audio=="ingame")
            this.GetComponent<AudioSource>().clip = audioClips[0];
        else if (audio == "lobby")
            this.GetComponent<AudioSource>().clip = audioClips[2];
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
