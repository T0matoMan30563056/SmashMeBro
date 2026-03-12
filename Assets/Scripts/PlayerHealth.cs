using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : NetworkBehaviour
{


    private float DamageTaken = 0;

    public float MaxHitpoints;
    public float Hitpoints;

    [Range(1f, 2f)] public float KnockbackScalar;

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
        Hitpoints = MaxHitpoints;


    }

    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Hurtbox") && !Invincibility && collision.GetComponent<DeleteHitbox>() != null)
        {
            
            DeleteHitbox Hitbox = collision.GetComponent<DeleteHitbox>();

            if (Hitbox.Origin == gameObject) return;

            try
            {
                if (Hitbox.Origin == collision.GetComponentInParent<PlayerMovement>().OwnerObject)
                {
                    DamageTaken += Mathf.Round(Hitbox.Damage);
                    StatUpdater.instance.StatHolderObj.Dmg += DamageTaken;
                    DamageTaken = 0f;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }


            Hitpoints -= Hitbox.Damage;

            if (Hitpoints <= 0f)
            {
                Death();
            }
            if (Hitbox.KnockbackFromCenter)
            {
                Vector3 direction = transform.position -Hitbox.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x);

                float CosFromCenter = Mathf.Cos(angle);
                float SinFromCenter = Mathf.Sin(angle);      


                rb.linearVelocity = new Vector2(Hitbox.KnockbackValue.x * CosFromCenter, Hitbox.KnockbackValue.y * SinFromCenter);
                if (playerMovement != null)
                {
                    playerMovement.MomentumTime = 0;
                    playerMovement.ExtraVertcalMomentum = collision.GetComponent<DeleteHitbox>().AddedVerticalMomentum * CosFromCenter;
                }
            }
            else
            {
                rb.linearVelocity = collision.GetComponent<DeleteHitbox>().KnockbackValue;
                if (playerMovement != null)
                {


                    playerMovement.MomentumTime = 0;
                    playerMovement.ExtraVertcalMomentum = collision.GetComponent<DeleteHitbox>().AddedVerticalMomentum;
                }
            }
            StartCoroutine(DamageBuffer());
        }
    }


    private void Death()
    {
        Hitpoints = MaxHitpoints;
        transform.position = transform.position.normalized;
    }



    private IEnumerator DamageBuffer()
    {
        Invincibility = true;
        yield return new WaitForSeconds(InvincibilityDuration);
        Invincibility = false;
    }

}
