using UnityEngine;
using System;
using System.Collections;

public class TestShoot : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private int BulletNum;
    [SerializeField] private float BurstCooldown;


    void Start()
    {
        animator = GetComponentInParent<PlayerAttacks>().animator;
        StartCoroutine(Shoot());
    }


    private IEnumerator Shoot()
    {
        for (int i = 0; i < BulletNum; i++)
        {

            animator.SetTrigger("Shooting");
            GameObject bullet = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
            ProjectileBehavior bulletBehaviour = bullet.GetComponent<ProjectileBehavior>();
            BulletParameters(bullet.GetComponent<DeleteHitbox>());

            bulletBehaviour.SetDirection(transform.localScale.x / Mathf.Abs(transform.localScale.x));




            yield return new WaitForSeconds(BurstCooldown);
        }
    }

    private void BulletParameters(DeleteHitbox bulletHitbox)
    {
        DeleteHitbox deleteHitbox = GetComponent<DeleteHitbox>();

        bulletHitbox.Damage = deleteHitbox.Damage;

        bulletHitbox.Origin = deleteHitbox.Origin;

        bulletHitbox.KnockbackValue = deleteHitbox.KnockbackValue;

        bulletHitbox.AddedVerticalMomentum = deleteHitbox.AddedVerticalMomentum;
    }

}
