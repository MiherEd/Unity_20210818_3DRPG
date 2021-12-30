using UnityEngine;

public class APIStaticSample : MonoBehaviour
{
    void Start()
    {
        print("Camera Count: " + Camera.allCamerasCount);
        print("2D Gravity: " + Physics2D.gravity);
        print("PI: " + Mathf.PI);

        Physics2D.gravity = new Vector2(0, -20);
        Time.timeScale = 0.5f;

        print("9.999 Round: " + Mathf.Round(9.999f));
        print("9.999 Floor: " + Mathf.FloorToInt(9.999f));

        Vector3 a = new Vector3(1, 1, 1);
        Vector3 b = new Vector3(22, 22, 22);
        print("Distance Between a And b: " + Vector3.Distance(a, b));

        Application.OpenURL("https://unity.com/");
    }

    void Update()
    {
        print("Input Any Key: " + Input.anyKey);
        print("Game Time: " + Time.time);
        print("Input Key Space: " + Input.GetKeyDown(KeyCode.Space));
    }
}
