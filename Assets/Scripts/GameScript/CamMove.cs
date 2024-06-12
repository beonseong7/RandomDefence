using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public GameObject Menu;
    public float base_speed = 2;
    public float cam_speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
   
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameManager.instance.Button_Active(Menu);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.transform.GetChild(9).gameObject.SetActive(false);
        }
        if (!Menu.activeSelf) this.MoveCam();


    }
    void MoveCam()
    {
        cam_speed = base_speed * (Screen.width/300);
        if (Input.mousePosition.x > Screen.width - (Screen.width / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(cam_speed, 0, 0), Space.World);
        }
        if (Input.mousePosition.x < (Screen.width / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(-cam_speed, 0, 0), Space.World);
        }
        if (Input.mousePosition.y > Screen.height - (Screen.height / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(0, 0, cam_speed), Space.World);
        }
        if (Input.mousePosition.y < (Screen.height / 30))
        {
            this.transform.GetComponent<Transform>().Translate(new Vector3(0, 0, -cam_speed), Space.World);
        }
    }
}
