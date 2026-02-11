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

    public AnimationCurve VerticalAnimation;
    public AnimationCurve HorizontalAnimation;
    public bool Animation;
    public bool Strafe;
    public float AddedVerticalMomentum;


    //public bool GivesJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DeleteWithDelay());
    }

    private IEnumerator DeleteWithDelay()
    {
        yield return new WaitForSeconds(Duration);

        DeleteOnServerRpc();
    }

    [ServerRpc]
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
