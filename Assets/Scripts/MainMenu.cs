using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;
    private bool Started = false;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LogInScene" && !Started)
        {
            StartCoroutine(DataBaseConnection.instance.StatsUpdater(10, 1, 15000));
            Started = true;
        }
    }

    public void Startgame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoggInTransfer()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void LoggIn()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(DataBaseConnection.instance.Login(username, password));
        Debug.Log(username + " " + password);
    }

    public void SignUpTransfer()
    {
        SceneManager.LoadSceneAsync(3);
    }


    public void SignUp()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(DataBaseConnection.instance.SignIn(username, password));
        Debug.Log(username + " " + password);
    }

}
