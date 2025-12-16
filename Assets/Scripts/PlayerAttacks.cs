using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private GameObject[] AttackSequence;
    [SerializeField] private GameObject[] LightUniqueAttacks;
    [SerializeField] private GameObject[] HeavyUniqueAttacks;

    private int AttackOrder = 0;
    [SerializeField] private float ResetTime;
    private float ResetTimeRemaining;

    public float Direction;
    public float VerticalDirection;

    public bool isGrounded;
    public bool isInside;

    private float RecoveryCooldown;

    private bool Recovery = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 

    }

    // Update is called once per frame
    void Update()
    {
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


    private void HeavyAttacks()
    {
        if (isGrounded)
        {
            if (Mathf.Abs(VerticalDirection) == 1)
            {
                if (isGrounded && !isInside && VerticalDirection == -1)
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

                GameObject HurtBoxObj = Instantiate(HeavyUniqueAttacks[0], Vector3.zero, Quaternion.identity, transform);
                HitboxParameters(HurtBoxObj);
            }
        } else {
            Debug.Log("you in the air gang :dead_rose:"); 
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
                    GameObject HurtBoxObj = Instantiate(LightUniqueAttacks[1], Vector3.zero, Quaternion.identity, transform);
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
            GameObject HurtBoxObj = Instantiate(AttackSequence[AttackOrder], Vector3.zero, Quaternion.identity, transform);
            HitboxParameters(HurtBoxObj);
            AttackOrder++;

            ResetTimeRemaining = ResetTime;
        }

        

        if (AttackOrder >= AttackSequence.Length)
        {
            AttackOrder = 0;
        }
    }

    private void HitboxParameters(GameObject HurtBoxObj)
    {
        DeleteHitbox HurboxHitbox = HurtBoxObj.GetComponent<DeleteHitbox>();

        HurtBoxObj.transform.localScale *= Direction;
        RecoveryCooldown = HurboxHitbox.Recovery;
        HurboxHitbox.KnockbackValue.x *= Direction;
        HurtBoxObj.transform.localPosition = new Vector3(HurboxHitbox.PositionValue.x * Direction, HurboxHitbox.PositionValue.y, 0f);

        Recovery = true;
    }

}
