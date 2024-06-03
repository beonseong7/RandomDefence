using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public Button Button;
    public GameObject[] Panels;
    Dictionary<string, string> Character_recipe=new Dictionary<string, string>();
    public static InGameManager instance = null;
    public int choice = 2;
    public GameObject[] Mob;
    public GameObject[] common;
    public GameObject[] Character_;
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
                tmp.transform.SetParent(Panels[0].transform);
            else if (recipe.Contains("R_"))
                tmp.transform.SetParent(Panels[1].transform);
            else if (recipe.Contains("E_"))
                tmp.transform.SetParent(Panels[2].transform);
            else if (recipe.Contains("L_"))
                tmp.transform.SetParent(Panels[3].transform);
            tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = recipe.Substring(0,recipe.Length-2);
        }
    }
    public void Random_summon()
    {
        var tmp=GameObject.Find("Player1Field").transform;
        if (choice > 0)
            Instantiate(common[UnityEngine.Random.Range(0, common.Length - 1)],new Vector3(tmp.position.x+UnityEngine.Random.Range(-10,10), 102,tmp.position.z + UnityEngine.Random.Range(-10, 10)),tmp.rotation);
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
