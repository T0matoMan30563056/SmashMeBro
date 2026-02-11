using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class StatUpdater : MonoBehaviour
{


    public class StatHolder
    {
        public int Kills = 0;
        public int Deaths = 0;
        public float Dmg = 0f;
    }

    public StatHolder StatHolderObj = new StatHolder();


    public static StatUpdater instance;
    public void Awake()
    {
        instance = this;
    }

    //If logged in start the StatSender coroute
    void Start()
    {
        Debug.Log(StatHolderObj);
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

            if (StatHolderObj.Kills != 0 || StatHolderObj.Deaths != 0 || StatHolderObj.Dmg != 0f)
            {
                StartCoroutine(DataBaseConnection.instance.StatsUpdater(StatHolderObj.Kills, StatHolderObj.Deaths, StatHolderObj.Dmg));
                Debug.Log(StatHolderObj.Kills);
                Debug.Log(StatHolderObj.Deaths);
                Debug.Log(StatHolderObj.Dmg);

                StatHolderObj = new StatHolder();
            }


            else
            {
                Debug.Log("Not sendt");
                Debug.Log(StatHolderObj.Kills);
                Debug.Log(StatHolderObj.Deaths);
                Debug.Log(StatHolderObj.Dmg);
            }
        }
    }
}
