using UnityEngine;

//Teleporterer tilbake alt som faller av kartet
//Hvis det som faller har PlayerMovements scriptet så sjekker vi om det er clienten som eier denne sessionen
//Hvis det er så legger vi til en Death på StatUpdater StatHolderObj
//Siden online ikke er ferdig netcoda så sjekker vi om Dummyen falt ned istedet
//Hvis den gjorde så legger vi til en kill på StatUpdater StatHolderObj
public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = default;

        if (collision.GetComponent<PlayerMovement>() != null)
        {
            if (collision.GetComponent<PlayerMovement>().OwnerObject == collision.gameObject)
            {
                StatUpdater.instance.StatHolderObj.Deaths += 1;
            }
        }
        else
        {
            //Temporary
            if (collision.name == "Dummy")
            {
                StatUpdater.instance.StatHolderObj.Kills += 1;
            }
        }
    }

}
