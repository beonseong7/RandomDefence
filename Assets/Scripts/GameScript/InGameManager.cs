using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public Button Button;
    public GameObject[] Panels;
    Dictionary<string, string> Character_recipe=new Dictionary<string, string>();
    public static InGameManager instance = null;
    public int choice = 10;
    public int stage = 1;
    public GameObject[] UI_Text;
    public GameObject[] Mob;
    public GameObject[] common;
    public GameObject Base_Uncommon;
    public GameObject MyCharacter;
    public GameObject Mob_Parents;
    public TextMeshProUGUI SystemMessage;
    public Dictionary<string,int> Character=new Dictionary<string, int>();
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
             int count = 1;
             foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
             {
                 GameObject.Find("Player" + count.ToString() + "Field").name = player.NickName.ToString() + "Field";
                 Debug.Log("Player in room: " + player.NickName);
            count++;
             }
        SystemMessage.text = "";
        string path = Path.Combine(Application.streamingAssetsPath, "recipe.txt");
        if (File.Exists(path))
        {
            string fileContent = File.ReadAllText(path);
            string[] recipes = fileContent.Split('\n');
            foreach(string recipe in recipes)
            {
                string[] tmp=recipe.Split(' ');
                Character_recipe.Add(tmp[0], tmp[1]);
            }   
        }
        else
        {
            Debug.LogError("The file does not exist.");
        }
        foreach(string recipe in Character_recipe.Keys)
        {
            var tmp = Instantiate(Button);
            tmp.name = recipe;
            
            tmp.onClick.AddListener(() => check_inventory(tmp.name));
            if (recipe.Contains("U_"))
                tmp.transform.SetParent(Panels[0].transform,true);
            else if (recipe.Contains("R_"))
                tmp.transform.SetParent(Panels[1].transform, true);
            else if (recipe.Contains("E_"))
                tmp.transform.SetParent(Panels[2].transform, true);
            else if (recipe.Contains("L_"))
                tmp.transform.SetParent(Panels[3].transform,true);
            tmp.GetComponent<RectTransform>().localPosition=new Vector3(tmp.GetComponent<RectTransform>().position.x, tmp.GetComponent<RectTransform>().position.y, 0);
            tmp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            tmp.GetComponent<RectTransform>().localScale = Button.GetComponent<RectTransform>().localScale;
            tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = recipe.Substring(0,recipe.Length-2);
            
        }
        StartCoroutine(summon_Mob());
    }
    IEnumerator summon_Mob()
    {
        yield return new WaitForSeconds(5f);
        while (stage < 9)
        {
            for (int i = 0; i < 40; i++)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
                    {
                        GameObject mob =PhotonNetwork.Instantiate("Mob_Prefab", GameObject.Find(player.NickName + "Field").transform.GetChild(3).position, Mob[stage - 1].transform.rotation);
                        PhotonView Local_photonView = mob.GetComponent<PhotonView>();
                        Local_photonView.RPC("SetField", RpcTarget.AllBuffered, player.NickName + "Field");
                        yield return null;
                    }
                       
                }
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(15f);
            stage++;
            choice += 2;
        }
    }
    IEnumerator SystemMs(string text)
    {
        SystemMessage.text = text;
        yield return new WaitForSeconds(5f);
        SystemMessage.text = "";
    }
    public void Random_summon()
    {
        var tmp=GameObject.Find(GameManager.instance.Nick_Name+"Field").transform;
        if (choice > 0)
        {
            var obj= Instantiate(common[UnityEngine.Random.Range(0, common.Length - 1)], new Vector3(tmp.position.x + UnityEngine.Random.Range(-10, 10), 102, tmp.position.z + UnityEngine.Random.Range(-10, 10)), tmp.rotation);
            obj.transform.GetComponent<Character_>().attack = 5.0f;
            obj.transform.SetParent(MyCharacter.transform);
            obj.name = obj.name.Split("(Clone)")[0];
            if (Character.ContainsKey(obj.name))
            {
                Character[obj.name]++;
            }
            else
            {
                Character.Add(obj.name, 1);
            }
            choice--;
        }
    }
    IEnumerator Summon(string type)
    {
        Debug.Log("시잉이발 됐다");
        var tmp = GameObject.Find("Player1Field").transform;
        var sacrifice = Character_recipe[type].Split(',');
        foreach (var mob in sacrifice)
        {
            Debug.Log(mob);
            var mob_tmp = mob.Trim();
            Destroy(MyCharacter.transform.Find(mob_tmp).gameObject);
            yield return null;
        }
        switch (type.Substring(type.Length - 2, 2))
        {
            case "U_":
                var obj = Instantiate(Base_Uncommon, new Vector3(tmp.position.x + UnityEngine.Random.Range(-10, 10), 102, tmp.position.z + UnityEngine.Random.Range(-10, 10)), tmp.rotation);
                obj.transform.GetComponent<Character_>().attack = 20.0f;
                obj.transform.SetParent(MyCharacter.transform);
                obj.name = type;
                break;
            case "R_":
                break;
            case "E_":
                break;
            case "L_":
                break;
            default:
                break;
        }
    }
    void check_inventory(string name)
    {
        var tmp = Character_recipe[name].Split(',');
        foreach (var item in tmp)
        {
            string t_item = item.Trim();
            if (Character.ContainsKey(t_item))
            {
                Character[t_item]--;
                if (Character[t_item] < 0)
                {
                    Character[t_item]++;
                    StartCoroutine(SystemMs(name + " 조합법 : " + Character_recipe[name]));
                    return;
                }
            }
            else
            {
                StartCoroutine(SystemMs(name + " 조합법 : " + Character_recipe[name]));
                return;
            } 
        }
        StartCoroutine(this.Summon(name));
    }
    public void Button_Active(GameObject tmp)
    {
        tmp.SetActive(!tmp.activeSelf);
    }
    public void OnGUI()
    {
        UI_Text[0].transform.GetComponent<TextMeshProUGUI>().text = "선택권 : " + choice.ToString();
        UI_Text[1].transform.GetComponent<TextMeshProUGUI>().text = "Stage : " + stage.ToString();
    }
    void Update()
    {
        
    }
}
