using UnityEngine;

public class API_Static : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region Static Property
        // Get
        // 語法: 類別名稱.靜態屬性
        float r = Random.value;
        print("取得靜態屬性 隨機值: " + r);

        // Set
        // 語法: 類別名稱.靜態屬性 指定 值;
        Cursor.visible = false;
        #endregion

        #region Static Method
        // 呼叫 參數、傳回
        // 簽章 參數、傳回
        // 語法: 類別名稱.靜態方法(對應引數)
        float rangeFloat = Random.Range(10.5f, 20.9f);
        print("浮點數隨機範圍 10.5 - 20.9: " + rangeFloat);

        int rangeInt = Random.Range(1, 5);
        print("整數隨機範圍 1 - 5: " + rangeInt);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region Static Property
        print("經過時間: " + Time.timeSinceLevelLoad);
        #endregion

        #region Static Method
        float h = Input.GetAxis("Horizontal");
        print("水平值: " + h);
        #endregion
    }
}
