using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public Button Button;
    public GameObject[] Panels;
    public string[,] Charac_Name =new string [,] { { "블루노","후쿠로","니코 로빈","베포","브룩","이나즈마","우솝","타시기","쵸파","페로나","에이스","프랑키","하찌"},{"루피"},{ "루피"},{"루피"} };
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
