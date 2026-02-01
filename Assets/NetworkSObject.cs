using UnityEngine;

[CreateAssetMenu(fileName = "NetworkSObject", menuName = "Scriptable Objects/NetworkSObject")]
public class NetworkSObject : ScriptableObject
{
    public class PlayerData
    {
        public string username;
        public string success;
    }
}
