using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;


//Håndterer kommunikasjon mellom unity og flask
//Lagrer klassene for andre skripts
public class DataBaseConnection : MonoBehaviour
{

    public static DataBaseConnection instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
    }
    

    [System.Serializable]
    public class PlayerData
    {
        public string SessionUsername;
        public bool Success;
    }

    public PlayerData playerData;



    //Gjør parametrene om til en Json
    //Lager en WebRequest til flask
    //Kjører Login funksjonen i flask
    //Uploader Jsonen til flask
    //Lagrer return verdien som en Json
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
            playerData = JsonUtility.FromJson<PlayerData>(request.downloadHandler.text);
            Debug.Log(playerData.SessionUsername);
            Debug.Log(playerData.Success);
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    //Gjør parametrene om til en Json
    //Lager en WebRequest til flask
    //Kjører SignUp funksjonen i flask
    //Uploader Jsonen til flask
    //Lagrer return verdien som en Json
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
            "http://10.200.14.25:5000/StatsUpdater",
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

    //Test funksjon for å teste tilkobling med flask
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

    [System.Serializable]
    class StatsData
    {
        public int Kills;
        public int Deaths;
        public float Dmg;
    }


}
