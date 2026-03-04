using TMPro;
using UnityEngine;


public class GetName : MonoBehaviour
{

    TMP_Text Text;


    //Spiller før start funksjoner
    //Sjekker hva DataBaseConnection playerData SessionUsername er
    //Hvis den er null eller tomt så blir navnet over spilleren Guest
    //Ellers så blir navnet til brukernavnet
    void Awake()
    {
        Text = GetComponent<TMP_Text>();


        if (DataBaseConnection.instance.playerData.SessionUsername == null || DataBaseConnection.instance.playerData.SessionUsername == string.Empty)
        {
            Text.text = "Guest";
        }
        else
        {
            Text.text = DataBaseConnection.instance.playerData.SessionUsername;
        }
    }

}
