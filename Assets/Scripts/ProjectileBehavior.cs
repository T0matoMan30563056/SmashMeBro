using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float BulletSpeed = 12f;

    void Update()
    {
    }

    public void SetDirection(float direction)
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * direction * BulletSpeed;
        Debug.Log(GetComponent<Rigidbody2D>().linearVelocity);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Hurtbox"))
        {
            Destroy(gameObject);
        }
    }

}