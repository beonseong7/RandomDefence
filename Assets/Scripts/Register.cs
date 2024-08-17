using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Register : MonoBehaviourPunCallbacks
{
    public InputField ID_Input;
    public InputField PW_Input;
    public InputField Email_Input;
    public InputField NickName_Input;
    public TextMeshProUGUI ErrorText;
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
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { ErrorText.text="Register Success";GameManager.instance.SetStat("ClearCount",0); GameManager.instance.SetData("Home", "Disconnect") ; }, (error)=>RegisterFailure(error));
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
