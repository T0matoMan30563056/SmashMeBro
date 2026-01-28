using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class DataBaseConnection : MonoBehaviour
{
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
    private void Start()
    {
        StartCoroutine(Login("t0mato", "NeGeR"));
        StartCoroutine(TestRequest());
    }

}
