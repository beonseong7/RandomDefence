using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Lobby : MonoBehaviourPunCallbacks
{
    public InputField input_roomname;
    public string roomname=null;
    public GameObject lobby;
    public GameObject gameroom;
    public GameObject createrooom;
    public GameObject btnObject;
    public Transform contents;
    public Text errorTt;
    public Dictionary<string,GameObject> Dic_rooms=new Dictionary<string, GameObject>() { };
    public void roomname_changed() => roomname = input_roomname.text.ToString();
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.Change_Audio("lobby");
        StartCoroutine(this.checkroom());
    }
    IEnumerator checkroom()
    {
        
        yield return new WaitForSeconds(3.0f);
        Debug.Log( PhotonNetwork.NetworkClientState);
        StartCoroutine(this.checkroom());
    }
    
    public void active_room(string roomname)
    {
        lobby.SetActive(roomname.Equals("lobby"));
        gameroom.SetActive(roomname.Equals("gameroom"));
        createrooom.SetActive(roomname.Equals("createroom"));
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(roomname, roomOptions);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        errorTt.text = message;
        Debug.Log("failed " + returnCode + "\n" + message);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        active_room("gameroom");
        Debug.Log("방생성 완료");
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       base.OnRoomListUpdate(roomList);
        Debug.Log(roomList.Count.ToString());
        
        foreach (var p in roomList)
        {
            if (p.RemovedFromList )
            {
                Debug.Log("Successsss");
                if(Dic_rooms.ContainsKey(p.Name))
                    Destroy(Dic_rooms[p.Name]);
                Dic_rooms.Remove(p.Name);
            }
            else if(p.IsOpen==true && !Dic_rooms.ContainsKey(p.Name))
            {
                add_room_Button(p.Name);
            }
            else if(p.IsOpen == false && Dic_rooms.ContainsKey(p.Name))
            {
                Destroy(Dic_rooms[p.Name]);
                Dic_rooms.Remove(p.Name);
            }
        }
    }
    public void add_room_Button(string tmp)
    {
        if (!Dic_rooms.ContainsKey(tmp))
        {
            GameObject btn = Instantiate(btnObject);
            btn.name = tmp;
            btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnClick_JoinBtn(tmp));
            btn.transform.GetChild(0).GetComponent<Text>().text = tmp;
            btn.transform.SetParent(contents);
            Dic_rooms.Add(tmp, btn);
        }
    }

    public void OnClick_JoinBtn(string tmp)
    {
        PhotonNetwork.JoinRoom(tmp);
    }
    public void Logout()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
    }
    
}
