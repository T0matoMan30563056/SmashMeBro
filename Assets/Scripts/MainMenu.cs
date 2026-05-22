using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//Dette scriptet inneholder funksjonene som blir kjørt av UI knapper
public class MainMenu : MonoBehaviour
{

    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField QuestionField;
    [SerializeField] GameObject HiddenContainer;
    [SerializeField] GetName getName;



    private bool Started = false;

    public static MainMenu instance;

    private void Awake()
    {

        instance = this;
        
    }

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
    public void LoggIn(TextMeshProUGUI ErrorText)
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(DataBaseConnection.instance.Login(username, password, ErrorText));
        Debug.Log(username + " " + password);
        usernameField.text = string.Empty;
        passwordField.text = string.Empty;
    }

    public void SignOut(TextMeshProUGUI ErrorText)
    {
        StartCoroutine(DataBaseConnection.instance.SignOut(ErrorText, getName));
    }

    //Laster sign up scenen
    public void SignUpTransfer()
    {
        SceneManager.LoadSceneAsync(3);
    }

    //Tar string variablene i input feltene
    //Sender dem til DataBaseConnection sin SignIn funksjon
    public void SignUp(TextMeshProUGUI ErrorText)
    {
        string username = usernameField.text;
        string password = passwordField.text;

        StartCoroutine(DataBaseConnection.instance.SignIn(username, password, ErrorText));
        Debug.Log(username + " " + password);

        usernameField.text = string.Empty;
        passwordField.text = string.Empty;
    }

    public void SendHelp(TextMeshProUGUI ErrorText)
    {
        string question = QuestionField.text;

        StartCoroutine(DataBaseConnection.instance.SendHelpMsg(question, ErrorText));
        Debug.Log(question);

        QuestionField.text = string.Empty;
    }

    public void SendDeleteRequest(TextMeshProUGUI ErrorText)
    {    
        string password = passwordField.text;

        StartCoroutine(DataBaseConnection.instance.DeleteUser(password, ErrorText));
        passwordField.text = string.Empty;

    }

    public void ShowContainer()
    {
        HiddenContainer.SetActive(true);
    }
    public void HideContainer()
    {
        HiddenContainer.SetActive(false);
    } 

    public void MainMenuTransfer()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void UserTransfer()
    {
        SceneManager.LoadSceneAsync(4);
    }


    public void FAQTransfer()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void HelpTransfer()
    {
        SceneManager.LoadSceneAsync(6);
    }

}
