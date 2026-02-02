using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

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


    private NetworkObject NetObj;

    private NetworkObjectReference NetObjRef;
    public override void OnNetworkSpawn()
    {
    }

    private void Start()
    {
        NetObj = GetComponent<NetworkObject>();
        NetObjRef = NetObj;
    }


void Update()
    {
        if (!IsOwner)
        {
            return;
        }
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
                RequestFireServerRpc(new int[] {1, 0}, NetObjRef, HeldDirection);
                //StartHitbox(LightUniqueAttacks[0], Vector3.zero, Quaternion.identity, transform);
            }
            else
            {
                if (isGrounded && !isInside)
                {
                    RequestFireServerRpc(new int[] { 1, 2 }, NetObjRef, HeldDirection);
                    //StartHitbox(LightUniqueAttacks[2], Vector3.zero, Quaternion.identity, transform);
                    
                }
                else
                {
                    RequestFireServerRpc(new int[] { 1, 1 }, NetObjRef, HeldDirection);
                    //StartHitbox(LightUniqueAttacks[1], Vector3.zero, Quaternion.identity, transform);
                }
            }
        }
        else
        {
            if (Mathf.Abs(Direction) == 1)
            {
                RequestFireServerRpc(new int[] { 1, 3 }, NetObjRef, HeldDirection);
                //StartHitbox(LightUniqueAttacks[3], Vector3.zero, Quaternion.identity, transform);
            }
            else
            {
                RequestFireServerRpc(new int[] { 0, AttackOrder}, NetObjRef, HeldDirection);
                //StartHitbox(AttackSequence[AttackOrder], Vector3.zero, Quaternion.identity, transform);
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
                    RequestFireServerRpc(new int[] { 2, 1 }, NetObjRef, HeldDirection);
                    //StartHitbox(HeavyUniqueAttacks[1], Vector3.zero, Quaternion.identity, transform);
                }
                else
                {
                    RequestFireServerRpc(new int[] { 2, 1 }, NetObjRef, HeldDirection);
                    //StartHitbox(HeavyUniqueAttacks[2], Vector3.zero, Quaternion.identity, transform);
                }
            }
            else
            {
                if (Mathf.Abs(Direction) == 1)
                {
                    RequestFireServerRpc(new int[] { 2, 5 }, NetObjRef, HeldDirection);
                    //StartHitbox(HeavyUniqueAttacks[5], Vector3.zero, Quaternion.identity, transform);
                }
                else
                {
                    RequestFireServerRpc(new int[] { 2, 0 }, NetObjRef, HeldDirection);
                    //StartHitbox(HeavyUniqueAttacks[0], Vector3.zero, Quaternion.identity, transform);
                }
            }
        } 
        else 
        {
            if (VerticalDirection == -1)
            {
                RequestFireServerRpc(new int[] { 2, 4 }, NetObjRef, HeldDirection);
                //StartHitbox(HeavyUniqueAttacks[4], Vector3.zero, Quaternion.identity, transform);
            }
            else
            {
                RequestFireServerRpc(new int[] { 2, 3 }, NetObjRef, HeldDirection);
                //StartHitbox(HeavyUniqueAttacks[3], Vector3.zero, Quaternion.identity, transform);
            }
        }
    }

   /*
    private void StartHitbox(GameObject Attack, Vector3 Position, Quaternion Rotation, Transform Origin)
    {
        GameObject HurtBoxObj = Instantiate(Attack, Position, Rotation, Origin);
        HitboxParameters(HurtBoxObj);
    }*/

    [ClientRpc]
    private void HitboxParametersClientRpc(NetworkObjectReference HurtBoxObjRef, float AttackDirection)
    {

        if (!HurtBoxObjRef.TryGet(out NetworkObject HurtBoxObj))
        {
            Debug.Log("No NetworkObj found");
            return;
        }

        DeleteHitbox HurboxHitbox = HurtBoxObj.GetComponent<DeleteHitbox>();

        HurtBoxObj.transform.localScale *= AttackDirection;
        RecoveryCooldown = HurboxHitbox.Recovery;
        HurboxHitbox.KnockbackValue.x *= AttackDirection;
        HurboxHitbox.AddedVerticalMomentum *= AttackDirection;
        HurtBoxObj.transform.localPosition = new Vector3(HurboxHitbox.PositionValue.x * AttackDirection, HurboxHitbox.PositionValue.y, 0f);
        HurboxHitbox.Origin = gameObject;

        if (HurboxHitbox.Animation)
        {
            GetComponent<PlayerMovement>().VerticalAnimation = HurboxHitbox.VerticalAnimation;
            GetComponent<PlayerMovement>().HorizontalAnimation = HurboxHitbox.HorizontalAnimation;
            GetComponent<PlayerMovement>().AnimationMovement(AttackDirection);
            GetComponent<PlayerMovement>().Strafe = HurboxHitbox.Strafe;
        }

        Recovery = true;
    }

    
    //Execute on server
    [ServerRpc]
    private void RequestFireServerRpc(int[] AttackType, NetworkObjectReference Origin, float AttackDirection)
    {
        //FireClientRpc(Attack, Position, Rotation, Origin);
        GameObject Attack = null;

        if (!Origin.TryGet(out NetworkObject OriginObj))
        {
            Debug.Log("No NetworkObj found");
            return;
        }

        if (AttackType[0] == 0)  // AttackType[0] = 0 means that the attack is part of the light attack sequence
        {
            Attack = AttackSequence[AttackType[1]];
        }
        else if (AttackType[0] == 1) // AttackType[0] = 1 means that the attack is an unique light attack
        {
            Attack = LightUniqueAttacks[AttackType[1]];
        }
        else if (AttackType[0] == 2) // AttackType[0] = 1 means that the attack is an unique heavy attack
        {
            Attack = HeavyUniqueAttacks[AttackType[1]];
        }

        if (Attack == null) 
        {
            return;
        }

        GameObject HurtBoxObj = Instantiate(Attack, OriginObj.transform.position, Quaternion.identity);
        NetworkObject AttackNetObj = HurtBoxObj.GetComponent<NetworkObject>();
        HurtBoxObj.GetComponent<NetworkObject>().Spawn();
        AttackNetObj.TrySetParent(OriginObj);

        NetworkObjectReference AttackObjRef = AttackNetObj;
        HitboxParametersClientRpc(AttackObjRef, AttackDirection);
    }
    /*
    //Execute on ALL clients
    [ClientRpc]
    private void FireClientRpc(GameObject Attack, Vector3 Position, Quaternion Rotation, Transform Origin)
    {
        if (IsOwner) return;
        GameObject HurtBoxObj = Instantiate(Attack, Position, Rotation, Origin);
        HitboxParameters(HurtBoxObj);
    }
    */
}
