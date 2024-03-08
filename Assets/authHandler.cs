using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class authHandler : MonoBehaviour
{
    private string url = "https://sid-restapi.onrender.com";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void enviarRegistro()
    {
        authData data = new authData(); 
        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Registro", JsonUtility.ToJson(data));
    }
    public void enviarLogin()
    {
        authData data = new authData();
        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Login", JsonUtility.ToJson(data));
    }
    IEnumerator Registro(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url+"/api/usuarios", json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Send Request Registro");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            Debug.Log(request.GetResponseHeader("Content-Type"));
            if (request.responseCode == 200)
            {
                Debug.Log("Registro Exitoso");
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator Login(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/auth/login", json);
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("Send Request Login");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            Debug.Log(request.GetResponseHeader("Content-Type"));
            if (request.responseCode == 200)
            {
                Debug.Log("Login Exitoso");

                authData data = JsonUtility.FromJson<authData>(request.downloadHandler.text);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}

public class authData
{
    public string username;
    public string password;
    public usuario user;
    public string token;
}
[System.Serializable]
public class usuario
{
    public string _id;
    public string user;
    public DataUser data;

    public usuario() 
    { 
        data = new DataUser();
    }
}

public class DataUser
{
    public int score;
}