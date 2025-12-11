using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI HeathText;

    private float DamageTaken = 0;

    private bool Invincibility = false;

    [SerializeField] private float InvincibilityDuration;

    void Update()
    {
        HeathText.text = DamageTaken.ToString();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Hurtbox") && !Invincibility)
        {
            DamageTaken += Mathf.Round(collision.GetComponent<DeleteHitbox>().Damage);
            StartCoroutine(DamageBuffer());
        }
    }

    private IEnumerator DamageBuffer()
    {
        Invincibility = true;
        yield return new WaitForSeconds(InvincibilityDuration);
        Invincibility = false;
    }

}
