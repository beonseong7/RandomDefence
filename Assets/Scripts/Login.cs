using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Login : MonoBehaviourPunCallbacks
{
    private string userID;
    private string password;
    public InputField ID_Input;
    public InputField PW_Input;
    public Text ErrorTt;
    private string _playFabPlayerIdCache;
    [Header("Disconnect")]
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();
    public InputField EmailInput, PasswordInput, UsernameInput;
    [Header("Lobby")]
    public InputField UserNickNameInput;
    public Text LobbyInfoText, UserNickNameText;
    [Header("Room")]
    public InputField SetDataInput;
    public GameObject SetDataBtnObj;
    public Text UserHouseDataText, RoomNameInfoText, RoomNumInfoText;
    bool isLoaded;
    public PhotonManager PhotonManager;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues();
    }
    public void ID_Value_changed()=> userID = ID_Input.text.ToString();
    public void PW_value_Changed()=> password = PW_Input.text.ToString();
    public void R_Login()
    {
        var request = new LoginWithPlayFabRequest { Username = userID, Password = password };
        PlayFabClientAPI.LoginWithPlayFab(request, (result) => GetUserData(result.PlayFabId), (error) => OnLoginFailure(error));
    }
    public void GetUserData(string playFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = playFabId }, results =>
        {
            Debug.Log("Got user data");
            if (results.Data == null || !results.Data.ContainsKey("Home")) ErrorTt.text = "잘못 등록된 계정입니다.";
            else
            {
                    GameManager.instance.SetData("Home", "Connect");
                    GetProfile(playFabId);
            }
        }, (error) => { ErrorTt.text = "없는계정입니다."; });
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        LogMessage(obj.GenerateErrorReport());
    }
    public void LogMessage(string message)
    {
        Debug.Log("PlayFab + Photon Example: " + message);
    }
    public void GetProfile(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(){
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints(){ShowDisplayName = true,}
        }, result =>{
            GameManager.instance.Nick_Name = result.PlayerProfile.DisplayName;
            Debug.Log(result.PlayerProfile.DisplayName);
        },(error)=> OnLoginFailure(error));

        Invoke("PlayfabAuthenticate", 1f);
    }
    
    void PlayfabAuthenticate()
    {
        //Debug.Log(GameManager.instance.Nick_Name.ToString());
        PhotonNetwork.AuthValues.UserId = userID;
        PhotonNetwork.LocalPlayer.NickName = GameManager.instance.Nick_Name;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("hi");
        Debug.Log(PhotonNetwork.CloudRegion);
        PhotonNetwork.LocalPlayer.NickName = GameManager.instance.Nick_Name;
        PhotonNetwork.JoinLobby();
    }
    void GetData(string curID)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = curID }, (result) =>
        UserHouseDataText.text = curID + "\n" + result.Data["Home"].Value,
        (error) => print("데이터 불러오기 실패"));
    }
    public override void OnJoinedLobby()
    {
        //LoadingSceneManager.LoadScene("Lobby");
        
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        //active_room("gameroom");
        Debug.Log("방생성 완료");
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("로그인 실패");
        Debug.LogWarning(error.GenerateErrorReport());
    }

}
