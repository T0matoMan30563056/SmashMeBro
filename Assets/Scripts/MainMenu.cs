using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//Dette scriptet inneholder funksjonene som blir kjørt av UI knapper
public class MainMenu : MonoBehaviour
{

    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;
    private bool Started = false;

    //Laster hoved scenen
    public void Startgame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    //Laster logg in scenen
    public void LoggInTransfer()
    {
        SceneManager.LoadSceneAsync(2);
    }

    //Tar string variablene i input feltene
    //Sender dem til DataBaseConnection sin Login funksjon
    public void LoggIn()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(DataBaseConnection.instance.Login(username, password));
        Debug.Log(username + " " + password);
    }

    //Laster sign up scenen
    public void SignUpTransfer()
    {
        SceneManager.LoadSceneAsync(3);
    }

    //Tar string variablene i input feltene
    //Sender dem til DataBaseConnection sin SignIn funksjon
    public void SignUp()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(DataBaseConnection.instance.SignIn(username, password));
        Debug.Log(username + " " + password);
    }

}
