using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class InGameManager : MonoBehaviour
{
    public Button Button;
    public GameObject[] Panels;
    Dictionary<string, string> Character_recipe=new Dictionary<string, string>();
    public static InGameManager instance = null;
    public int choice = 2;
    public int stage = 1;
    public GameObject[] UI_Text;
    public GameObject[] Mob;
    public GameObject[] common;
    public List<GameObject> Character_=new List<GameObject>();
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
       /*if(PhotonNetwork.IsMasterClient)
        {
            int count = 1;
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                GameObject.Find("Player" + count.ToString() + "Field").name = player.NickName.ToString() + "Field";
                Debug.Log("Player in room: " + player.NickName);
            }
        }*/
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
        while (stage < 9)
        {
            for (int i = 0; i < 40; i++)
            {
                var mob= Instantiate(Mob[stage - 1]);
                mob.transform.GetComponent<MobScript>().mob_Hp=10.0f;
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(20f);
            stage++;
            choice += 2;
        }
    }
    public void Random_summon()
    {
        var tmp=GameObject.Find("Player1Field").transform;
        if (choice > 0)
        {
            var obj= Instantiate(common[UnityEngine.Random.Range(0, common.Length - 1)], new Vector3(tmp.position.x + UnityEngine.Random.Range(-10, 10), 102, tmp.position.z + UnityEngine.Random.Range(-10, 10)), tmp.rotation);
            obj.transform.GetComponent<Character_>().attack = 5.0f;
        }
        choice--;
    }
    void check_inventory(string name)
    {
        Debug.Log(Character_recipe[name]);
    }
    public void Button_Active(GameObject tmp)
    {
        tmp.SetActive(!tmp.activeSelf);
    }
    public void OnGUI()
    {
        UI_Text[0].transform.GetComponent<TextMeshProUGUI>().text = "º±≈√±« : " + choice.ToString();
        UI_Text[1].transform.GetComponent<TextMeshProUGUI>().text = "Stage : " + stage.ToString();
    }
    void Update()
    {
        
    }
}
