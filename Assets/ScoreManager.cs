using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    private string url = "https://sid-restapi.onrender.com";
    private usuario Usuario;
    private string Token;
    void Start()
    {
        Usuario = new usuario();
        Usuario.user = PlayerPrefs.GetString("username");
        Token = PlayerPrefs.GetString("token");
    }

    public void ActualizarScore(int score)
    {
        Usuario.data.score = score;
        StartCoroutine("SetScore", JsonUtility.ToJson(Usuario));
    }
    IEnumerator SetScore(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios", json);
        request.method = "PATCH";
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("X-Token", Token);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.GetResponseHeader("Content-Type"));
            Debug.Log(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                Debug.Log("SetScore Exitoso");
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }
}
