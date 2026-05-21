using UnityEngine;

public class CheckLoginState : MonoBehaviour
{
    [SerializeField] private GameObject[] ToBeHiddenArr;
    void Start()
    {
        Debug.Log(DataBaseConnection.instance.playerData.SessionUsername);
        if (DataBaseConnection.instance.playerData.SessionUsername == null || DataBaseConnection.instance.playerData.SessionUsername == string.Empty)
        {
            Debug.Log("Running");

            foreach (GameObject Obj in ToBeHiddenArr) 
            {
                Obj.SetActive(false);
            }
        }
    }

    void Update()
    {
        
    }
}