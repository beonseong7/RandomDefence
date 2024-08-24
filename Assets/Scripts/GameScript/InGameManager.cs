using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static UnityEditor.Progress;
using UnityEngine.Networking;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public Button Button;
    public GameObject[] Panels;
    public Dictionary<string, string> Character_recipe = new Dictionary<string, string>();
    public static InGameManager instance = null;
    public int choice = 20;
    public int stage = 1;
    private int timer = 0;
    public GameObject[] UI_Text;
    public GameObject[] Mob;
    public GameObject[] common;
    public GameObject Base_Uncommon;
    public GameObject MyCharacter;
    public GameObject Mob_Parents;
    public TextMeshProUGUI SystemMessage;
    public Dictionary<string, int> Character = new Dictionary<string, int>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    void Start()
    {
        SystemMessage.text = "";
        string path = Path.Combine(Application.streamingAssetsPath, "recipe.txt");
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            string[] recipes = fileContent.Split('\n');
            foreach (string recipe in recipes)
            {
                string[] tmp = recipe.Split(' ');
                Character_recipe.Add(tmp[0], tmp[1]);
            }
        }
        else
        {
            Debug.LogError("The file does not exist.");
        }
        foreach (string recipe in Character_recipe.Keys)
        {
            var tmp = Instantiate(Button);
            tmp.name = recipe;
            tmp.onClick.AddListener(() => check_inventory(tmp.name));
            if (recipe.Contains("_U"))
                tmp.transform.SetParent(Panels[0].transform, true);
            else if (recipe.Contains("_R"))
                tmp.transform.SetParent(Panels[1].transform, true);
            else if (recipe.Contains("_E"))
                tmp.transform.SetParent(Panels[2].transform, true);
            else if (recipe.Contains("_L"))
                tmp.transform.SetParent(Panels[3].transform, true);
            tmp.GetComponent<RectTransform>().localPosition = new Vector3(tmp.GetComponent<RectTransform>().position.x, tmp.GetComponent<RectTransform>().position.y, 0);
            tmp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            tmp.GetComponent<RectTransform>().localScale = Button.GetComponent<RectTransform>().localScale;
            tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = recipe.Substring(0, recipe.Length - 2);

        }

        if (PhotonNetwork.IsMasterClient)
        {
            int count = 1;
            foreach (Player name in PhotonNetwork.CurrentRoom.Players.Values)
            {
                photonView.RPC("Field_name", RpcTarget.All, count, name.ActorNumber);
                count++;
            }
        }
        GameManager.instance.Change_Audio("ingame");
        StartCoroutine(this.Stage_Manage());
        
    }
    [PunRPC]
    public void Field_name(int count, int actornum)
    {
        GameObject.Find("Player" + count.ToString() + "Field").name = actornum.ToString() + "Field";

    }
    IEnumerator Stage_Manage()
    {
        yield return new WaitForSeconds(5f);
        Camera.main.transform.position = new Vector3(GameObject.Find(PhotonNetwork.LocalPlayer.ActorNumber + "Field").transform.position.x, Camera.main.transform.position.y, GameObject.Find(PhotonNetwork.LocalPlayer.ActorNumber + "Field").transform.position.z);
        StartCoroutine(this.time());
        while (stage < 9)
        {
                timer = 55;
                for (int i = 0; i < 40; i++)
                {
                if (PhotonNetwork.IsMasterClient) photonView.RPC("Summon_Mob", RpcTarget.All);
                yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(15f);
                if (PhotonNetwork.IsMasterClient) photonView.RPC("Next_Stage", RpcTarget.All);
        }
        StartCoroutine(this.Save());
        if (GameObject.Find("MyMobs").transform.childCount < 55)
            Invoke("GameClear", 3.0f);
    }
    IEnumerator Save()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://beonseong7.dothome.co.kr/php/Update_ClearDt.php?NickName=" + GameManager.instance.Nick_Name))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    void GameClear()
    {
        if (GameObject.Find("MyMobs").transform.childCount < 55)
        {
            Panels[4].SetActive(true);
            UI_Text[6].transform.GetComponent<TextMeshProUGUI>().text = "저장 완료";
        }
    }
    
    IEnumerator time()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(1.0f);
            timer--;
            photonView.RPC("Set_Timer", RpcTarget.Others, timer);
            StartCoroutine(time());
        }
        else
        {
            yield return null;
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(this.time());
        }
    }
    [PunRPC]
    public void Next_Stage()
    {
        stage++;
        choice += 2;
    }
    [PunRPC]
    public void Set_Timer(int time)
    {
        timer = time;
    }
    [PunRPC]
    public void Summon_Mob()
    {
        GameObject mob = PhotonNetwork.Instantiate("Mob_Prefab", GameObject.Find(PhotonNetwork.LocalPlayer.ActorNumber + "Field").transform.GetChild(3).position, Mob[stage - 1].transform.rotation);
    }
    public void SystemMs(string text)
    {
        SystemMessage.text = text;
    }
    public void ClearSystemMs()
    {
        SystemMessage.text = "";
    }
    public void Random_summon()
    {
        var tmp=GameObject.Find(PhotonNetwork.LocalPlayer.ActorNumber+"Field").transform;
        if (choice > 0)
        {
            var obj= PhotonNetwork.Instantiate("Character/Common/"+common[UnityEngine.Random.Range(0, common.Length - 1)].name, new Vector3(tmp.position.x + UnityEngine.Random.Range(-10, 10), 5, tmp.position.z + UnityEngine.Random.Range(-10, 10)), tmp.rotation);
            obj.transform.SetParent(MyCharacter.transform);
            var name=obj.name.Split("(Clone)")[0];
            if (Character.ContainsKey(name))
            {
                Character[name]++;
            }
            else
            {
                Character.Add(name, 1);
            }
            choice--;
        }
    }
    void Summon(string type)
    {
        var tmp = GameObject.Find(PhotonNetwork.LocalPlayer.ActorNumber+"Field").transform;
        var sacrifice = Character_recipe[type].Split(',');
        GameObject obj;
        
        foreach (var mob in sacrifice)
        {
            Debug.Log(mob);
            var mob_tmp = mob.Trim();
            MyCharacter.transform.Find(mob_tmp).GetComponent<PhotonView>().RPC("Cha_Destroy", RpcTarget.All);

        }
        if (Character.ContainsKey(type))
        {
            Character[type]++;
        }
        else
        {
            Character.Add(type, 1);
        }
        switch (type.Substring(type.Length - 2, 2))
        {
            case "_U":
                obj = PhotonNetwork.Instantiate("Character/Rair/sword_shield", new Vector3(tmp.position.x + UnityEngine.Random.Range(-10, 10), 5, tmp.position.z + UnityEngine.Random.Range(-10, 10)), tmp.rotation);
                obj.transform.SetParent(MyCharacter.transform);
                //obj.name = type;
                break;
            case "_R":
                obj = PhotonNetwork.Instantiate("Character/Uncommon/archor_", new Vector3(tmp.position.x + UnityEngine.Random.Range(-10, 10), 5, tmp.position.z + UnityEngine.Random.Range(-10, 10)), tmp.rotation);
                obj.transform.SetParent(MyCharacter.transform);
                //obj.name = type;
                break;
            case "_E":
                break;
            case "_L":
                break;
            default:
                break;
        }
    }
    public void check_inventory(string name)
    {
        var tmp = Character_recipe[name].Split(',');
        Dictionary<string, int> charac_tmp = new Dictionary<string, int>(Character);
        foreach (var item in tmp)
        {
            string t_item = item.Trim();
            if (charac_tmp.ContainsKey(t_item))
            {
                if (charac_tmp[t_item] <= 0)
                {
                    return;
                }
                charac_tmp[t_item]--;
            }
            else
            {
                return;
            }
        }
        Character = charac_tmp;
        this.Summon(name);
    }
    public void Button_Active(GameObject tmp)
    {
        tmp.SetActive(!tmp.activeSelf);
    }
    public void Watching()
    {

    }
    public void Lobby()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void OnGUI()
    {
        string character_tt="";
        foreach(var item in Character)
        {
            if(item.Value>0) character_tt += item.Key + " X " + item.Value + "\n";
        }
        UI_Text[0].transform.GetComponent<TextMeshProUGUI>().text = "선택권 : " + choice.ToString();
        UI_Text[1].transform.GetComponent<TextMeshProUGUI>().text = "Stage : " + stage.ToString();
        UI_Text[2].transform.GetComponent<TextMeshProUGUI>().text = GameManager.instance.Nick_Name;
        UI_Text[3].transform.GetComponent<TextMeshProUGUI>().text = "Timer : " + timer.ToString();
        UI_Text[4].transform.GetComponent<TextMeshProUGUI>().text = "MY MOB:" + GameObject.Find("MyMobs").transform.childCount.ToString();
        UI_Text[5].transform.GetComponent<TextMeshProUGUI>().text = character_tt;
        if (GameObject.Find("MyMobs").transform.childCount > 54)
        {
            Panels[4].SetActive(true);
            UI_Text[6].transform.GetComponent<TextMeshProUGUI>().text = "저장 완료";
            Debug.Log("GameOver");
        }
            
    }
    void Update()
    {
        
    }
}
