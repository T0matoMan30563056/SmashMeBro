using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Unity.VisualScripting;
using System;
using UnityEditor.PackageManager.Requests;

public class DataBaseConnection : MonoBehaviour
{

    public static DataBaseConnection instance;
    public NetworkSObject.PlayerData SObjPlayerData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public IEnumerator Login(string username, string password)
    {
        string json = JsonUtility.ToJson(new LoginData
        {
            username = username,
            password = password
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);


        UnityWebRequest request = new UnityWebRequest(
            "http://10.200.14.25:5000/login",
            "POST"
        );
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            SObjPlayerData = JsonUtility.FromJson<NetworkSObject.PlayerData>(request.downloadHandler.text);
            print(SObjPlayerData.ToString());
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    public IEnumerator SignIn(string username, string password)
    {
        string json = JsonUtility.ToJson(new LoginData
        {
            username = username,
            password = password
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);


        UnityWebRequest request = new UnityWebRequest(
            "http://10.200.14.25:5000/SignUp",
            "POST"
        );
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }


    public IEnumerator TestRequest()
    {
        UnityWebRequest request = new UnityWebRequest(
            "http://10.200.14.25:5000/",
            "POST"
        );
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();


        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    [System.Serializable]
    class LoginData
    {
        public string username;
        public string password;
    }


}
