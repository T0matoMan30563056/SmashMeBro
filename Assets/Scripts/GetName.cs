using TMPro;
using UnityEngine;


public class GetName : MonoBehaviour
{

    TMP_Text Text;

    [SerializeField] private string AdditionalCharacters;
    [SerializeField] private string Default;


    //Spiller før start funksjoner
    //Sjekker hva DataBaseConnection playerData SessionUsername er
    //Hvis den er null eller tomt så blir navnet over spilleren Guest
    //Ellers så blir navnet til brukernavnet
    void Awake()
    {
        GetNameFunc();
    }

    public void GetNameFunc()
    {
        Text = GetComponent<TMP_Text>();


        if (DataBaseConnection.instance.playerData.SessionUsername == null || DataBaseConnection.instance.playerData.SessionUsername == string.Empty)
        {
            Text.text = Default;
        }
        else
        {
            Text.text = AdditionalCharacters + DataBaseConnection.instance.playerData.SessionUsername;
        }
    }


}
