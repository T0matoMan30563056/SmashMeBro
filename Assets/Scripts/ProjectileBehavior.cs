using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float BulletSpeed = 12f;
    public float Lifetime = 3f;
    private Rigidbody2D rb;

    void Update()
    {
        Destroy(gameObject, Lifetime);
    }

    public void SetDirection(float direction)
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * direction * BulletSpeed;
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