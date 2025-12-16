using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;


public class DeleteHitbox : MonoBehaviour
{
    [SerializeField] private float Duration;
    public float Damage;
    public float Recovery;
    public Vector2 KnockbackValue;
    public Vector2 PositionValue;
    public GameObject Origin;
    //public bool GivesJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DeleteWithDelay());
    }

    private IEnumerator DeleteWithDelay()
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }

    /*
    public void GainJump()
    {
        if (!GivesJump) return;
        Origin.GetComponent<PlayerMovement>().AirJump = true;
    }
    */
}
