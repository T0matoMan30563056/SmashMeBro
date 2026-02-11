using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = default;

        if (collision.GetComponent<PlayerMovement>() == null) return;

        if (collision.GetComponent<PlayerMovement>().OwnerObject == collision.gameObject)
        {
            StatUpdater.instance.StatHolderObj.Deaths += 1;
        }
    }

}
