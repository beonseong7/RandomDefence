using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public GameObject Menu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
   
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InGameManager.instance.Button_Active(Menu);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var tmp = GameObject.Find("Canvas");
            tmp.transform.GetChild(2).gameObject.SetActive(false);
        }
        if (!Menu.activeSelf) this.MoveCam();


    }
    void MoveCam()
    {
        if (Input.mousePosition.x > Screen.width - (Screen.width / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(3, 0, 0), Space.World);
        }
        if (Input.mousePosition.x < (Screen.width / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(-3, 0, 0), Space.World);
        }
        if (Input.mousePosition.y > Screen.height - (Screen.height / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(0, 0, 3), Space.World);
        }
        if (Input.mousePosition.y < (Screen.height / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(0, 0, -3), Space.World);
        }
    }
}
