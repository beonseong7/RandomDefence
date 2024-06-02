using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public Button Button;
    public GameObject[] Panels;
    public Dictionary<string, string> Charac_Name;
    void Start()
    {
        foreach(var tmp in Panels)
        while (tmp.transform.childCount<14)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
