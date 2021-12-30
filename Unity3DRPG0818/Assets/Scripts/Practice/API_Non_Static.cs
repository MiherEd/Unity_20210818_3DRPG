using UnityEngine;

public class API_Non_Static : MonoBehaviour
{
    public Transform tra1;  // 修飾詞 要存取非靜態的類別 欄位名稱
    public Camera cam;
    public Light lig;
    void Start()
    {
        #region Non-Static Property
        // 與靜態差異
        // 1. 需要實體物件
        // 2. 取得實體物件 - 定義欄位並將要存取的物件存入欄位
        // Get
        // 語法: 欄位名稱.非靜態屬性
        print("Camera Position: " + tra1.position);
        print("Camera Depth: " + cam.depth);

        // Set
        // 語法: 欄位名稱.非靜態屬性 指定 值
        tra1.position = new Vector3(99, 99, 99);
        cam.depth = 7;
        #endregion

        #region Non-Static Method
        // 呼叫
        // 語法: 欄位名稱.非靜態方法名稱(對應引數)
        lig.Reset();
        #endregion
    }

    void Update()
    {
        
    }
}
