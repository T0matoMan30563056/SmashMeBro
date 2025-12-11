using UnityEngine;
using System.Collections;


public class DeleteHitbox : MonoBehaviour
{
    [SerializeField] private float Duration;
    public float Damage;
    public float Recovery;


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
