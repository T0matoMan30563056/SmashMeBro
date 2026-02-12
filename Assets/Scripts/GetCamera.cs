using UnityEngine;


//Legger Camera.main til å være worldCameraet til spillerens canvas siden den er i en prefab
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
