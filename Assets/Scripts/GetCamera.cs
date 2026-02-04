using UnityEngine;

public class GetCamera : MonoBehaviour
{
    private Canvas canvas;
    private Camera camera;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        camera = Camera.main;
        canvas.worldCamera = camera;
    }
}
