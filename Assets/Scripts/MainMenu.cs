using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void Startgame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LogIn()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void SignUp()
    {
        SceneManager.LoadSceneAsync(3);
    }

}
