using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class PlayerAttacks : NetworkBehaviour
{
    [SerializeField] private Animator animator;
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
                InitAttack(new int[] { 1, 0 });
                //StartHitbox(LightUniqueAttacks[0], Vector3.zero, Quaternion.identity, transform);

            }
            else
            {
                if (isGrounded && !isInside)
                {
                    InitAttack(new int[] { 1, 2 });
                    //StartHitbox(LightUniqueAttacks[2], Vector3.zero, Quaternion.identity, transform);


                }
                else
                {
                    InitAttack(new int[] { 1, 1 });
                    //StartHitbox(LightUniqueAttacks[1], Vector3.zero, Quaternion.identity, transform);
   
                }
            }
        }
        else
        {
            if (Mathf.Abs(Direction) == 1)
            {
                InitAttack(new int[] { 1, 3 });
                //StartHitbox(LightUniqueAttacks[3], Vector3.zero, Quaternion.identity, transform

            }
            else
            {
                InitAttack(new int[] { 0, AttackOrder });
                //StartHitbox(AttackSequence[AttackOrder], Vector3.zero, Quaternion.identity, transform);
                AttackOrder++;
                animator.SetFloat("AttackCombo", AttackOrder);
                animator.SetTrigger("LightAttack");


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
                    InitAttack(new int[] { 2, 1 });
                    //StartHitbox(HeavyUniqueAttacks[1], Vector3.zero, Quaternion.identity, transform);

                }
                else
                {
                    InitAttack(new int[] { 2, 2 });
                    //StartHitbox(HeavyUniqueAttacks[2], Vector3.zero, Quaternion.identity, transform);
                }
            }
            else
            {
                if (Mathf.Abs(Direction) == 1)
                {
                    InitAttack(new int[] { 2, 5 });
                    //StartHitbox(HeavyUniqueAttacks[5], Vector3.zero, Quaternion.identity, transform);
                }
                else
                {
                    InitAttack(new int[] { 2, 0 });
                    //StartHitbox(HeavyUniqueAttacks[0], Vector3.zero, Quaternion.identity, transform);
                }
            }
        } 
        else 
        {
            if (VerticalDirection == -1)
            {
                InitAttack(new int[] { 2, 4 });
                //StartHitbox(HeavyUniqueAttacks[4], Vector3.zero, Quaternion.identity, transform);
            }
            else
            {
                InitAttack(new int[] { 2, 3 });
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

    private void HitboxParameters(GameObject HurtBoxObj)
    {



        DeleteHitbox HurtboxHitbox = HurtBoxObj.GetComponent<DeleteHitbox>();

        HurtBoxObj.transform.localScale *= HeldDirection;
        RecoveryCooldown = HurtboxHitbox.Recovery;
        HurtboxHitbox.KnockbackValue.x *= HeldDirection;
        HurtboxHitbox.AddedVerticalMomentum *= HeldDirection;
        HurtboxHitbox.transform.localPosition = new Vector3(HurtboxHitbox.PositionValue.x * HeldDirection, HurtboxHitbox.PositionValue.y, 0f);
        HurtboxHitbox.Origin = gameObject;
        HurtboxHitbox.LocalHitbox = true;

        if (HurtboxHitbox.Animation)
        {
            GetComponent<PlayerMovement>().VerticalAnimation = HurtboxHitbox.VerticalAnimation;
            GetComponent<PlayerMovement>().HorizontalAnimation = HurtboxHitbox.HorizontalAnimation;
            GetComponent<PlayerMovement>().AnimationMovement(HeldDirection);
            GetComponent<PlayerMovement>().Strafe = HurtboxHitbox.Strafe;
        }

        Recovery = true;
    }


    private void InitAttack(int[] AttackType)
    {
        Debug.Log(AttackType[0]);
        Debug.Log(AttackType[1]);

        if (!IsServer)
        {
            GameObject Attack = null;

            if (AttackType[0] == 0)  // AttackType[0] = 0 means that the attack is part of the light attack sequence
            {
                Attack = AttackSequence[AttackType[1]];
            }
            else if (AttackType[0] == 1) // AttackType[0] = 1 means that the attack is an unique light attack
            {
                Attack = LightUniqueAttacks[AttackType[1]];
            }
            else if (AttackType[0] == 2) // AttackType[0] = 2 means that the attack is an unique heavy attack
            {
                Attack = HeavyUniqueAttacks[AttackType[1]];
            }

            if (Attack == null)
            {
                return;
            }


            GameObject HurtBoxObj = Instantiate(Attack, Vector3.zero, Quaternion.identity, transform);
            HitboxParameters(HurtBoxObj);
        }


        RequestFireServerRpc(AttackType, NetObjRef, HeldDirection);
    }





    [Rpc(SendTo.SpecifiedInParams)]
    private void HitboxParametersClientRpc(NetworkObjectReference HurtBoxObjRef, float AttackDirection, RpcParams rpcParams = default)
    {

        if (!HurtBoxObjRef.TryGet(out NetworkObject HurtBoxObj))
        {
            Debug.Log("No NetworkObj found");
            return;
        }

        DeleteHitbox HurtboxHitbox = HurtBoxObj.GetComponent<DeleteHitbox>();

        HurtBoxObj.transform.localScale *= AttackDirection;
        RecoveryCooldown = HurtboxHitbox.Recovery;
        HurtboxHitbox.KnockbackValue.x *= AttackDirection;
        HurtboxHitbox.AddedVerticalMomentum *= AttackDirection;
        HurtboxHitbox.transform.localPosition = new Vector3(HurtboxHitbox.PositionValue.x * AttackDirection, HurtboxHitbox.PositionValue.y, 0f);
        HurtboxHitbox.Origin = gameObject;

        if (HurtboxHitbox.Animation)
        {
            GetComponent<PlayerMovement>().VerticalAnimation = HurtboxHitbox.VerticalAnimation;
            GetComponent<PlayerMovement>().HorizontalAnimation = HurtboxHitbox.HorizontalAnimation;
            GetComponent<PlayerMovement>().AnimationMovement(AttackDirection);
            GetComponent<PlayerMovement>().Strafe = HurtboxHitbox.Strafe;
        }

        Recovery = true;
    }

    
    //Execute on server
    [Rpc(SendTo.Server)]
    private void RequestFireServerRpc(int[] AttackType, NetworkObjectReference Origin, float AttackDirection, RpcParams rpcParams = default)
    {

        //Gets the client id that called the serverrpc to later exclude them from the clientrpc
        ulong senderClientId = rpcParams.Receive.SenderClientId;

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
        else if (AttackType[0] == 2) // AttackType[0] = 2 means that the attack is an unique heavy attack
        {
            Attack = HeavyUniqueAttacks[AttackType[1]];
        }

        if (Attack == null) 
        {
            return;
        }

        GameObject HurtBoxObj = Instantiate(Attack, OriginObj.transform.position, Quaternion.identity);
        NetworkObject AttackNetObj = HurtBoxObj.GetComponent<NetworkObject>();
        AttackNetObj.Spawn();
        AttackNetObj.TrySetParent(OriginObj);
        NetworkObjectReference AttackObjRef = AttackNetObj;


        //Checks if the sender is the server
        //if its the server that requested the RPC then it simply proceeds to the client RPC
        //Otherwise the client RPC gets called for everyone exept the sender
        if (senderClientId == 0)
        {
            HitboxParametersClientRpc(AttackObjRef, AttackDirection, RpcTarget.Everyone);
        }
        else
        {
            AttackNetObj.NetworkHide(senderClientId);
            HitboxParametersClientRpc(AttackObjRef, AttackDirection, RpcTarget.Not(senderClientId, RpcTargetUse.Temp));
        }
    }

}
