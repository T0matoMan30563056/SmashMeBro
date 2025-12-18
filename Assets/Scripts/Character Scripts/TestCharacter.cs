using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    /*
    private Rigidbody2D rb;
    private bool AnimationRunning;

    private Vector2 AnimationMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (AnimationRunning)
        {
            rb.linearVelocity = AnimationMovement;
        }
    }

    public IEnumerator StraightMovement(Vector2 MovementDirection, float Duration)
    {

        AnimationMovement = MovementDirection;
        AnimationRunning = true;
        GetComponent<PlayerMovement>().AnimationStun = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        yield return new WaitForSeconds(Duration);

        rb.gravityScale = originalGravity;

        GetComponent<PlayerMovement>().AnimationStun = false;
        AnimationRunning = false;
    }*/
}
