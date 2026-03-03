using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public float FollowSpeed = 1f;
    public Vector2 Offset = new Vector2(0f, 1.5f);
    public Transform target;
    public Transform dummy;


    void Update()
    {
        //if (target == null) return;
        //Vector3 newPos = new Vector3(
        //target.position.x + Offset.x,
        //target.position.y + Offset.y, -10f);
        // https://music.youtube.com/playlist?list=PLPEoCU-WIrOQ2v0d51a1o10TMOZ9VDNsW&si=9pZg4rEV9Fj0duJu
        //transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);

        if (target == null) return;
        if (dummy == null) return;

        Vector3 MidPunkt = (target.position + dummy.position) / 2;
        Vector3 newPos = new Vector3(MidPunkt.x + Offset.x, MidPunkt.y + Offset.y, -10f);

        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}