using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AuthManager : MonoBehaviour
{
    private string url = "https://sid-restapi.onrender.com";
    public string Token { get; set; }
    public string Username { get; set; }
    public GameObject PanelAuth;
    public GameObject PanelMenu;
    // Start is called before the first frame update
    void Start()
    {
        Token = PlayerPrefs.GetString("token"); 
        if(string.IsNullOrEmpty(Token))
        {
            Debug.Log("No hay token");
            PanelAuth.SetActive(true);
        }

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
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
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
                Username = data.user.user;
                Token = data.token;

                PlayerPrefs.SetString("token", Token);
                PlayerPrefs.SetString("user", Username);

                PanelAuth.SetActive(false);
                PanelMenu.SetActive(true);
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator getProfile()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/usuarios" + Username);
        request.SetRequestHeader("x-token", Token);
        Debug.Log("Send Request GetProfile");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                Debug.Log("Get Profile Exitoso");
                authData data = JsonUtility.FromJson<authData>(request.downloadHandler.text);
                Debug.Log("El usuario" + data.user.user  + "se encuentra registrado y su puntaje es " + data.user.data.score);
                PanelMenu.SetActive(true);
            }
            else
            {
                PanelAuth.SetActive(true);

                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}

