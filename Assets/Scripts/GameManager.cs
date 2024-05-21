using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using DI = System.Diagnostics;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private string nick_Name;

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
    // Update is called once per frame
    void Update()
    {
    }
}
