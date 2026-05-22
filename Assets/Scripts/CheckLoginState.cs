using UnityEngine;

public class CheckLoginState : MonoBehaviour
{
    [SerializeField] private GameObject[] ToBeHiddenArr;


    public static CheckLoginState instance;

    private void Awake()
    {

        instance = this;

    }

    void Start()
    {
        CheckEachLoginState();
    }

    public void CheckEachLoginState()
    {
        Debug.Log(DataBaseConnection.instance.playerData.SessionUsername);
        if (DataBaseConnection.instance.playerData.SessionUsername == null || DataBaseConnection.instance.playerData.SessionUsername == string.Empty)
        {
            foreach (GameObject Obj in ToBeHiddenArr)
            {
                Obj.SetActive(false);
            }
        }
    }
}