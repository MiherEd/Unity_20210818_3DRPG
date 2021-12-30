using UnityEngine;

public class APIStaticPractice : MonoBehaviour
{
    void Start()
    {
        int cameraCount = Camera.allCamerasCount;
        print("所有攝影機數量：" + cameraCount);

        Vector2 gravity2D = Physics2D.gravity;
        gravity2D = new Vector2(0, -20f);
        Physics2D.gravity = gravity2D;
        print("2D重力：" + gravity2D);

        float pi = Mathf.PI;
        print("圓周率：" + pi);  
        
        Time.timeScale = 0.5f;
        print("時間速度：" + Time.timeScale);

        print("9.999無條件捨去：" + Mathf.FloorToInt(9.999f));

        print("兩點距離：" + Vector3.Distance(new Vector3(1, 1, 1), new Vector3(22, 22, 22)));

        Application.OpenURL("https://unity.com/");

    }

    void Update()
    {
        bool anyKey = (Input.anyKey);
        print(anyKey);
        bool space = Input.GetKeyDown(KeyCode.Space);
        print(space);
        print("經過時間：" + Time.timeSinceLevelLoad);
    }
}
