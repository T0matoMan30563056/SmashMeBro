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

}
