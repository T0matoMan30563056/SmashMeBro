using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


//Håndterer kommunikasjon mellom unity og flask
//Lagrer klassene for andre skripts
public class DataBaseConnection : MonoBehaviour
{

    public string ServerIP;

    public static DataBaseConnection instance;

    [SerializeField] private GameObject LoadingPanel;

    private GameObject LoadingPanelObj;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }


    [System.Serializable]
    public class PlayerData
    {
        public string SessionUsername;
        public bool Success;
        public byte[] pfp;
    }

    public PlayerData playerData;

    [System.Serializable]
    public class NotificationData
    {
        public string Type;
        public string Message;
        public bool Is_Read;
        public DateTime Created_At;
        public bool Clickable;
        public string ClickableJSON;
    }
    public NotificationData[] notificationData;

    //Gjør parametrene om til en Json
    //Lager en WebRequest til flask
    //Kjører Login funksjonen i flask
    //Uploader Jsonen til flask
    //Lagrer return verdien som en Json
    public IEnumerator Login(string username, string password, TextMeshProUGUI ErrorText)
    {
        LoadStart();
        string json = JsonUtility.ToJson(new LoginData
        {
            username = username,
            password = password
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);


        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/login",
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
            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                playerData = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
                Debug.Log(playerData.SessionUsername);
                Debug.Log(playerData.Success);
                MainMenu.instance.MainMenuTransfer();
            }
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        LoadStop();
    }

    //Gjør parametrene om til en Json
    //Lager en WebRequest til flask
    //Kjører SignUp funksjonen i flask
    //Uploader Jsonen til flask
    //Lagrer return verdien som en Json
    public IEnumerator SignIn(string username, string password, TextMeshProUGUI ErrorText)
    {
        LoadStart();
        string json = JsonUtility.ToJson(new LoginData
        {
            username = username,
            password = password
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);


        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/SignUp",
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
            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                playerData = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
                MainMenu.instance.MainMenuTransfer();
            }
            Debug.Log(playerData.SessionUsername);
            Debug.Log(playerData.Success);
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        LoadStop();
    }


    public IEnumerator SignOut(TextMeshProUGUI ErrorText, GetName getName)
    {
        LoadStart();
        string json = JsonUtility.ToJson(new DeleteRequest
        {
            password = String.Empty
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);


        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/SignOut",
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
            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                playerData = new PlayerData();
                CheckLoginState.instance.CheckEachLoginState();
                getName.GetNameFunc();
            }
            Debug.Log(playerData.SessionUsername);
            Debug.Log(playerData.Success);
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        LoadStop();
    }


    //Gjør parametrene om til en Json
    //Lager en WebRequest til flask
    //Kjører StatUpdater funksjonen i flask
    //Uploader Jsonen til flask
    //Lagrer return verdien som en Json
    public IEnumerator StatsUpdater(int kills, int deaths, float dmg)
    {
        Debug.Log("Sendt!");
        string json = JsonUtility.ToJson(new StatsData
        {
            Kills = kills,
            Deaths = deaths,
            Dmg = dmg
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/StatsUpdater",
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
            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
            }
        }

    }

    public IEnumerator ProfileUpdater(string NewUsername, Texture2D PFP, TextMeshProUGUI ErrorText)
    {
        LoadStart();
        Debug.Log("Updating profile");

        PFP = ResizeTexture(PFP, 128, 128);

        byte[] pfpByteArray = PFP.EncodeToJPG();

        string json = JsonUtility.ToJson(new UserData
        {
            newUsername = NewUsername,
            pfp = pfpByteArray
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/EditAccount",
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

            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                string HeldUsername = playerData.SessionUsername;
                playerData = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
                if (playerData.SessionUsername == null || playerData.SessionUsername == string.Empty) playerData.SessionUsername = HeldUsername;
                MainMenu.instance.MainMenuTransfer();
            }
        }
        LoadStop();
    }


    public IEnumerator SearchFriend(string FriendUsername, TextMeshProUGUI ErrorText, Action<string, byte[]> returningData)
    {
        LoadStart();
        Debug.Log("Locating friend");

        string json = JsonUtility.ToJson(new FriendCheck
        {
            friendUsername = FriendUsername,
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/SearchFriend",
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

            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                FriendData foundFriend = JsonUtility.FromJson<FriendData>(request.downloadHandler.text);
                returningData?.Invoke(foundFriend.friendUsername, foundFriend.friendPfp);
            }
        }
        LoadStop();
    }

    public IEnumerator SendFriendRequest(string FriendUsername, TextMeshProUGUI ErrorText)
    {
        LoadStart();
        Debug.Log("Sending request");

        string json = JsonUtility.ToJson(new FriendData
        {
            friendUsername = FriendUsername,
            
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/FriendRequest",
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

            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                string foundFriend = JsonUtility.FromJson<FriendData>(request.downloadHandler.text).friendUsername;
                Debug.Log("Sendt friend request to " + FriendUsername);
            }
        }
        LoadStop();
    }


    public IEnumerator SendHelpMsg(string HelpMessage, TextMeshProUGUI ErrorText)
    {
        LoadStart();
        string json = JsonUtility.ToJson(new HelpRequest
        {
            question_txt = HelpMessage
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/HelpRequest",
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
            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                MainMenu.instance.FAQTransfer();
            }
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        LoadStop();
    }


    public IEnumerator GetNotifications(bool MustBeRead /*, TextMeshProUGUI ErrorText*/)
    {
        LoadStart();
        string json = JsonUtility.ToJson(new Notifications
        {
            MustBeRead = MustBeRead,
            username = playerData.SessionUsername
        });

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/GetNotifications",
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
            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {

                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                //if (!MustBeRead) ErrorText.text = ErrorMsg;
            }
            else
            {
                if (MustBeRead) UnreadMessageExtraction(request.downloadHandler.text);
                else MessageExtraction(request.downloadHandler.text);
                //
                // Continue Here Later!!!
                //
            }
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        LoadStop();
    }
    private void UnreadMessageExtraction(string downloadHandlerText)
    {
        Debug.Log(downloadHandlerText);
    }
    private void MessageExtraction(string downloadHandlerText)
    {
        Debug.Log(downloadHandlerText);
    }

    public IEnumerator DeleteUser(string password, TextMeshProUGUI ErrorText)
    {
        LoadStart();
        string json = JsonUtility.ToJson(new DeleteRequest
        {
            password = password
        });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/DeleteAccount",
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
            var DownloadHandlerJson = JObject.Parse(request.downloadHandler.text);
            bool SuccessState = (bool)DownloadHandlerJson["success"];
            if (!SuccessState)
            {
                string ErrorMsg = (string)DownloadHandlerJson["error"];
                Debug.Log(ErrorMsg);
                ErrorText.text = ErrorMsg;
            }
            else
            {
                playerData = new PlayerData();
                ErrorText.text = String.Empty;
                CheckLoginState.instance.CheckEachLoginState();
                MainMenu.instance.HideContainer();
            }
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        LoadStop();
    }

    //Test funksjon for å teste tilkobling med flask
    public IEnumerator TestRequest()
    {
        UnityWebRequest request = new UnityWebRequest(
            "http://" + ServerIP + "/",
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

    [System.Serializable]
    class StatsData
    {
        public int Kills;
        public int Deaths;
        public float Dmg;
    }

    [System.Serializable]
    class UserData
    {
        public string newUsername;
        public byte[] pfp;
    }

    [System.Serializable]
    class HelpRequest
    {
        public string question_txt;
    }

    [System.Serializable]
    class DeleteRequest
    {
        public string password;
    }

    [System.Serializable]
    class Notifications
    {
        public bool MustBeRead;
        public string username;
    }

    [System.Serializable]
    class FriendData
    {
        public string friendUsername;
        public byte[] friendPfp;
    }

    [System.Serializable]
    class FriendCheck
    {
        public string friendUsername;
    }

    //Resizes texture by rendering it on a low resolutionrender texture
    //Then saving it to a new texture2D that replaces the old texture
    private Texture2D ResizeTexture(Texture2D InputTexture, int Width, int Height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(Width, Height);
        Graphics.Blit(InputTexture, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(Width, Height);
        result.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
        result.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }

    private void LoadStart()
    {
        GameObject canvas = GameObject.Find("UI");

        LoadingPanelObj = Instantiate(LoadingPanel, Vector3.zero, Quaternion.identity);

        LoadingPanelObj.transform.SetParent(canvas.transform);

        LoadingPanelObj.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        LoadingPanelObj.GetComponent<RectTransform>().offsetMax = Vector2.one;

    }

    private void LoadStop()
    {
        Destroy(LoadingPanelObj);
    }

}
