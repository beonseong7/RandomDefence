using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using TMPro;
public class GameRoom: MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI Chat;
    public TMP_InputField Chatfield;
    public ScrollRect scroll_rect;
    public Transform Player_Panel;
    public GameObject Player_img;
    public Button ReadyBtn;
    public Button StartBtn;
    public Text errorTt;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }
    public void EndSubmit()
    {
        if (Chatfield.text.Equals(""))
        {
            Debug.Log("chatEmpty");
            return;
        }
        string msg = string.Format("[{0}]:{1}",GameManager.instance.Nick_Name, Chatfield.text);
        Debug.Log(msg);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered,msg);
        ReceiveMsg(msg);
        
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("방참가 완료");
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
        transform.GetComponent<Lobby>().active_room("gameroom");
        foreach (var tmp in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(tmp.ToString());
            Add_Player(tmp.Value.NickName.ToString());
        }
        CheckHost();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        errorTt.text = message;
    }
    public void CheckHost()
    {
        Player_Panel.Find(PhotonNetwork.MasterClient.NickName.ToString()).GetChild(1).GetComponent<TextMeshProUGUI>().text = "HOST";
        if (PhotonNetwork.IsMasterClient)
        {
            StartBtn.interactable = true;
            ReadyBtn.interactable = false;
        }
        else
        {
            StartBtn.interactable = false;
            ReadyBtn.interactable = true;
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Add_Player(newPlayer.NickName);
        CheckHost();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Destroy(Player_Panel.Find(otherPlayer.NickName).gameObject);
        CheckHost();
    }
    public void Add_Player(string nickname)
    {
        var tmp = Instantiate(Player_img);
        tmp.name = nickname;
        tmp.transform.SetParent( Player_Panel);
        tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = nickname;
    }
    public void ReadyOrNot()
    {
        photonView.RPC("Update_Player_list", RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer.NickName.ToString());
        Update_Player_list(PhotonNetwork.LocalPlayer.NickName.ToString());
    }
    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        Chat.text += "\n" + msg;
        Chatfield.text = "";
    }
    [PunRPC]
    public void Update_Player_list(string nickname)
    {
        var tmp = Player_Panel.Find(nickname).GetChild(1).GetComponent<TextMeshProUGUI>();
            if (tmp.text.Equals("WAIT"))
            {
                tmp.text = "READY";
            }
            else
            {
                tmp.text = "WAIT";
            }
        Debug.Log(tmp.text);
    }
    IEnumerator ScrollUpdate()
    {
        yield return null;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
    // Update is called once per frame
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        transform.GetComponent<Lobby>().active_room("lobby");
        var tmp = Player_Panel.GetComponentsInChildren<Transform>();
        for(int i = 1; i < tmp.Length; i++)
        {
            Destroy(tmp[i].gameObject);
        }
    }
    public void Press_Start()
    {
        foreach(Transform tmp in Player_Panel.transform)
        {
            Debug.Log(tmp.GetChild(1).GetComponent<TextMeshProUGUI>().text);
            if (tmp.GetChild(1).GetComponent<TextMeshProUGUI>().text == "WAIT") return;
        }
            
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;
        photonView.RPC("StartGame", RpcTarget.AllBuffered);
       
    }
    [PunRPC]
    void StartGame()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
