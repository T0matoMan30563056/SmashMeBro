using UnityEngine;
using System.Collections;
using Unity.Netcode;



public class DeleteHitbox : NetworkBehaviour
{
    [SerializeField] private float Duration;
    public float Damage;
    public float Recovery;
    public Vector2 KnockbackValue;
    public Vector2 PositionValue;
    public GameObject Origin;
    public bool WeLoveFemboys = true;
    public AnimationCurve VerticalAnimation;
    public AnimationCurve HorizontalAnimation;
    public bool Animation;
    public bool Strafe;
    public float AddedVerticalMomentum;

    public bool LocalHitbox = false;

    //public bool GivesJump;

    void Start()
    {
        StartCoroutine(DeleteWithDelay());
    }

    private IEnumerator DeleteWithDelay()
    {
        yield return new WaitForSeconds(Duration);

        if (LocalHitbox)
        {
            Destroy(gameObject);
        }
        else
        {
            DeleteOnServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void DeleteOnServerRpc()
    {
        NetworkObject.Despawn(true);
    }

    /*
    public void GainJump()
    {
        if (!GivesJump) return;
        Origin.GetComponent<PlayerMovement>().AirJump = true;
    }
    */
}
