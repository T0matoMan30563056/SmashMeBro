using TMPro;
using UnityEngine;

public class GetName : MonoBehaviour
{
    TMP_Text Text;

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
