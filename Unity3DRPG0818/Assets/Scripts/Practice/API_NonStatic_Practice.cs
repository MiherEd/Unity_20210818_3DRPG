using UnityEngine;

public class API_NonStatic_Practice : MonoBehaviour
{
    public Camera cam;    
    public SpriteRenderer bird1;
    public Transform bird2;
    public Rigidbody2D bird3;
    
    void Start()
    {
        print("Camera Depth: " + cam.depth);
        print("Sprite Renderer: " + bird1.color);

        cam.backgroundColor = Random.ColorHSV();
        bird1.flipY = true;
    }

    void Update()
    {
        bird2.Rotate(0, 0, 3);
        bird3.AddForce(new Vector2(0, 10));
    }
}
