using UnityEngine;

public class GetNotifications : MonoBehaviour
{
    [SerializeField] private bool OnlyGetUnread;
    void Start()
    {
        if (OnlyGetUnread) GetUndread();
        else GetAllNotifications();
    }

    private void GetUndread()
    {

    }

    private void GetAllNotifications()
    {
        StartCoroutine(DataBaseConnection.instance.GetNotifications(false));
    }
}
