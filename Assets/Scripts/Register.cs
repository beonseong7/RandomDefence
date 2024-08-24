using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;

public class Register : MonoBehaviourPunCallbacks
{
    public TMP_InputField ID_Input;
    public TMP_InputField PW_Input;
    public TMP_InputField Email_Input;
    public TMP_InputField NickName_Input;
    public TextMeshProUGUI ErrorText;
    private string userID;
    private string password;
    private string email;
    private string nickname;

 
    void Start()
    {
    }
    public void ID_Value_changed()=> userID = ID_Input.text.ToString();
    public void PW_value_Changed() => password = PW_Input.text.ToString();
    public void Email_value_Changed()=> email = Email_Input.text.ToString();
    public void NickName_Changed() => nickname = NickName_Input.text.ToString();
    
    public IEnumerator H_Register()
    {
        string url = "http://beonseong7.dothome.co.kr/php/Register.php";

        // POST 데이터를 담을 WWWForm 생성
        WWWForm form = new WWWForm();
        form.AddField("Id", userID);
        form.AddField("Pw", password);
        form.AddField("Email", email);
        form.AddField("NickName", nickname);

        // POST 요청 생성
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            // 요청 전송
            yield return www.SendWebRequest();
            // 오류 체크
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                if(www.downloadHandler.text=="Fail")
                    RegisterFailure(www.downloadHandler.text);
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }
    public void R_Register()
    {
        StartCoroutine(H_Register());
    }

    
    private void RegisterFailure(string error)
    {
        Debug.LogWarning("가입 실패");
        Debug.Log(userID);
        Debug.Log(password);
        Debug.Log(nickname);
        Debug.Log(email);
        Debug.LogWarning(error);
        ErrorText.text = error;
    }
}
