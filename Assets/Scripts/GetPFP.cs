using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetPFP : MonoBehaviour
{
    Texture2D PFP;

    [SerializeField] RawImage ImageHolder;
    [SerializeField] TMP_InputField usernameField;
    private void Start()
    {
        LoadImage();
    }

    public void LoadImage()
    {
        Texture2D PFP = new Texture2D(2, 2);
        PFP.LoadImage(DataBaseConnection.instance.playerData.pfp);
        print(DataBaseConnection.instance.playerData.pfp);
        ImageHolder.texture = PFP;
    }


    public void OpenFileBrowser()
    {
        var extentions = new[]
        {
            new ExtensionFilter("ImageFiles", "png", "jpg", "jpeg")
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extentions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            Debug.Log("Selected file path: " + paths[0]);
            byte[] fileData = System.IO.File.ReadAllBytes(paths[0]);
            PFP = new Texture2D(2, 2);
            PFP.LoadImage(fileData);

            ImageHolder.texture = PFP;

        }
    }

    public void SendChanges()
    {
        Debug.Log("Trying to send...");
        if (PFP != null)
        {
            Debug.Log("Sendt!");

            string NewUsername = usernameField.text;
            
            if (NewUsername == string.Empty)
            {
                StartCoroutine(DataBaseConnection.instance.ProfileUpdater(DataBaseConnection.instance.playerData.SessionUsername, PFP));
            }
            else
            {
                StartCoroutine(DataBaseConnection.instance.ProfileUpdater(NewUsername, PFP));
            }
        }
        else
        {
            Debug.Log("Failed to send!");
        }
    }
}
