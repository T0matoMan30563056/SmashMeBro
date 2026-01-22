using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : NetworkBehaviour
{
    [SerializeField] private GameObject[] AttackSequence;
    [SerializeField] private GameObject[] LightUniqueAttacks;
    [SerializeField] private GameObject[] HeavyUniqueAttacks;

    private int AttackOrder = 0;
    [SerializeField] private float ResetTime;
    private float ResetTimeRemaining;


    private float HeldDirection = 1;
    public float Direction;
    public float VerticalDirection;

    public bool isGrounded;
    public bool isInside;

    private float RecoveryCooldown;

    private bool Recovery = false;




    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Direction) == 1)
        {
            HeldDirection = Direction;
        }
        if (GetComponent<PlayerMovement>().isDashing)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !Recovery)
        {
            LightAttacks();
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame && !Recovery)
        {
            HeavyAttacks();

        }

        ResetTimeRemaining = Mathf.Max(ResetTimeRemaining - Time.deltaTime, 0);

        if (ResetTimeRemaining == 0)
        {
            AttackOrder = 0;
        }


        RecoveryCooldown = Mathf.Max(RecoveryCooldown - Time.deltaTime, 0);

        if (RecoveryCooldown == 0)
        {
            Recovery = false;
        }

    }

    private void LightAttacks()
    {

        if (Mathf.Abs(VerticalDirection) == 1)
        {
            if (VerticalDirection == 1)
            {
                GameObject HurtBoxObj = Instantiate(LightUniqueAttacks[0], Vector3.zero, Quaternion.identity, transform);
                HitboxParameters(HurtBoxObj);
            }
            else
            {
                if (isGrounded && !isInside)
                {
                    GameObject HurtBoxObj = Instantiate(LightUniqueAttacks[2], Vector3.zero, Quaternion.identity, transform);
                    HitboxParameters(HurtBoxObj);
                }
                else
                {
                    GameObject HurtBoxObj = Instantiate(LightUniqueAttacks[1], Vector3.zero, Quaternion.identity, transform);
                    HitboxParameters(HurtBoxObj);
                }
            }
        }
        else
        {
            if (Mathf.Abs(Direction) == 1)
            {
                GameObject HurtBoxObj = Instantiate(LightUniqueAttacks[3], Vector3.zero, Quaternion.identity, transform);
                HitboxParameters(HurtBoxObj);
            }
            else
            {
                GameObject HurtBoxObj = Instantiate(AttackSequence[AttackOrder], Vector3.zero, Quaternion.identity, transform);
                HitboxParameters(HurtBoxObj);
                AttackOrder++;

                ResetTimeRemaining = ResetTime;
            }

        }



        if (AttackOrder >= AttackSequence.Length)
        {
            AttackOrder = 0;
        }
    }

    private void HeavyAttacks()
    {
        if (isGrounded && !isInside)
        {
            if (Mathf.Abs(VerticalDirection) == 1)
            {
                if (VerticalDirection == -1)
                {
                    GameObject HurtBoxObj = Instantiate(HeavyUniqueAttacks[1], Vector3.zero, Quaternion.identity, transform);
                    HitboxParameters(HurtBoxObj);
                }
                else
                {
                    GameObject HurtBoxObj = Instantiate(HeavyUniqueAttacks[2], Vector3.zero, Quaternion.identity, transform);
                    HitboxParameters(HurtBoxObj);
                }
            }
            else
            {
                if (Mathf.Abs(Direction) == 1)
                {
                    GameObject HurtBoxObj = Instantiate(HeavyUniqueAttacks[5], Vector3.zero, Quaternion.identity, transform);
                    HitboxParameters(HurtBoxObj);
                }
                else
                {
                    GameObject HurtBoxObj = Instantiate(HeavyUniqueAttacks[0], Vector3.zero, Quaternion.identity, transform);
                    HitboxParameters(HurtBoxObj);
                }
            }
        } 
        else 
        {
            if (VerticalDirection == -1)
            {
                GameObject HurtBoxObj = Instantiate(HeavyUniqueAttacks[4], Vector3.zero, Quaternion.identity, transform);
                HitboxParameters(HurtBoxObj);
            }
            else
            {
                GameObject HurtBoxObj = Instantiate(HeavyUniqueAttacks[3], Vector3.zero, Quaternion.identity, transform);
                HitboxParameters(HurtBoxObj);
            }
        }
    }

   
    private void HitboxParameters(GameObject HurtBoxObj)
    {
        DeleteHitbox HurboxHitbox = HurtBoxObj.GetComponent<DeleteHitbox>();

        HurtBoxObj.transform.localScale *= HeldDirection;
        RecoveryCooldown = HurboxHitbox.Recovery;
        HurboxHitbox.KnockbackValue.x *= HeldDirection;
        HurboxHitbox.AddedVerticalMomentum *= HeldDirection;
        HurtBoxObj.transform.localPosition = new Vector3(HurboxHitbox.PositionValue.x * HeldDirection, HurboxHitbox.PositionValue.y, 0f);
        HurboxHitbox.Origin = gameObject;

        if (HurboxHitbox.Animation)
        {
            GetComponent<PlayerMovement>().VerticalAnimation = HurboxHitbox.VerticalAnimation;
            GetComponent<PlayerMovement>().HorizontalAnimation = HurboxHitbox.HorizontalAnimation;
            GetComponent<PlayerMovement>().AnimationMovement(HeldDirection);
            GetComponent<PlayerMovement>().Strafe = HurboxHitbox.Strafe;
        }

        Recovery = true;
    }

}
