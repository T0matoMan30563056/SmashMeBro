using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class StatUpdater : MonoBehaviour
{


    public class StatHolder
    {
        public int Kills;
        public int Deaths;
        public float Dmg;
    }

    public StatHolder StatHolderObj;


    public static StatUpdater instance;
    public void Awake()
    {
        instance = this;
    }

    //If logged in start the StatSender coroute
    void Start()
    {
        if (DataBaseConnection.instance.playerData.SessionUsername != null || DataBaseConnection.instance.playerData.SessionUsername != string.Empty)
        {
            StartCoroutine(StatSender());
        }
    }

    //Går i en loop som kjører hvert 10 sekund
    //Kjører ikke hvis StatHolderObj har default verdier for ikke å sende ubrukelig data
    //Sender StatHolderObj til DataBaseConnection StatsUpdater
    //Gjør StatHolderObj til sine default verider igjen
    private IEnumerator StatSender()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            if (StatHolderObj != default(StatHolder))
            {
                DataBaseConnection.instance.StatsUpdater(StatHolderObj.Kills, StatHolderObj.Deaths, StatHolderObj.Dmg);
                Debug.Log("Sendt!");
                StatHolderObj = default;
            }
            else Debug.Log("Not sendt");
        }
    }
}
