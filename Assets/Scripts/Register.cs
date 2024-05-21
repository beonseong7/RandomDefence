using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Register : MonoBehaviourPunCallbacks
{
    public InputField ID_Input;
    public InputField PW_Input;
    public InputField Email_Input;
    public InputField NickName_Input;
    public Text ErrorText;
    private string userID;
    private string password;
    private string email;
    private string nickname;

 
    void Start()
    {
        PlayFabSettings.TitleId = "DC7BA";
    }
    public void ID_Value_changed()=> userID = ID_Input.text.ToString();
    public void PW_value_Changed() => password = PW_Input.text.ToString();
    public void Email_value_Changed()=> email = Email_Input.text.ToString();
    public void NickName_Changed() => nickname = NickName_Input.text.ToString();
    
    public void R_Register()
    {
        var request = new RegisterPlayFabUserRequest { Username = userID, Password = password, DisplayName=nickname,Email=email};
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("회원가입 성공");SetStat(); GameManager.instance.SetData("Home", "Disconnect") ; }, (error)=>RegisterFailure(error));
    }

    void SetStat()
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDINfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error) => print("값 저장실패"));
    }
    
    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("가입 실패");
        Debug.Log(userID);
        Debug.Log(password);
        Debug.Log(nickname);
        Debug.Log(email);
        Debug.LogWarning(error.GenerateErrorReport());
        ErrorText.text = error.GenerateErrorReport();
    }
}
