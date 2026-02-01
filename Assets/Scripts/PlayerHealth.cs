using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : NetworkBehaviour
{


    private float DamageTaken = 0;

    private bool Invincibility = false;

    [SerializeField] private float InvincibilityDuration;

    private Rigidbody2D rb;

    public bool Stunned = false;

    private PlayerMovement playerMovement;



    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            GetComponent<PlayerInput>().enabled = false;
            enabled = false;
            return;
        }
    }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Hurtbox") && !Invincibility && collision.GetComponent<DeleteHitbox>() != null)
        {
            if (collision.GetComponent<DeleteHitbox>().Origin == gameObject) return;


            //collision.GetComponent<DeleteHitbox>().GainJump();
            DamageTaken += Mathf.Round(collision.GetComponent<DeleteHitbox>().Damage);
            rb.linearVelocity = collision.GetComponent<DeleteHitbox>().KnockbackValue;
            if (playerMovement != null)
            {
                playerMovement.MomentumTime = 0;
                playerMovement.ExtraVertcalMomentum = collision.GetComponent<DeleteHitbox>().AddedVerticalMomentum;
            }
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
