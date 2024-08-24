using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;

public class Login : MonoBehaviourPunCallbacks
{
    private string userID="12312";
    private string password="123123";
    public TMP_InputField ID_Input;
    public TMP_InputField PW_Input;
    public TextMeshProUGUI ErrorTt;
    private string _playFabPlayerIdCache;
    [Header("Disconnect")]
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
        GameManager.instance.Change_Audio("main");
        PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues();
    }
    public void ID_Value_changed()=> userID = ID_Input.text.ToString();
    public void PW_value_Changed()=> password = PW_Input.text.ToString();
     public IEnumerator H_Login()
    {
        Debug.Log(userID + password);
        using(UnityWebRequest www = UnityWebRequest.Get("http://beonseong7.dothome.co.kr/php/Login.php?Id=" + userID + "&Pw=" + password))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                OnLoginFailure(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if ( www.downloadHandler.text=="0")OnLoginFailure(www.error);
                else
                {
                    this.Set_Connect();
                    GameManager.instance.Nick_Name = www.downloadHandler.text;
                }
            }
        }
    }
    public void R_Login()
    {
        StartCoroutine(H_Login());
    }
    public void LogMessage(string message)
    {
        Debug.Log("PlayFab + Photon Example: " + message);
    }
    
    void Set_Connect()
    {
        //Debug.Log(GameManager.instance.Nick_Name.ToString());
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
    public override void OnJoinedLobby()
    {
        //LoadingSceneManager.LoadScene("Lobby");
        
        PhotonNetwork.LoadLevel(1);
    }
    private void OnLoginFailure(string error)
    {
        Debug.LogWarning("로그인 실패");
        Debug.LogWarning(error);
    }

}
