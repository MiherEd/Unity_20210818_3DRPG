using UnityEngine;  // 引用 API
using UnityEngine.Video;


namespace coffee.Practice
{
    /// <summary>
    /// Coffee 20210906
    /// 第三人稱控制器
    /// 移動、跳躍
    /// </summary>

    // 修飾詞 類別 類別名稱 : 繼承類別
    // 欄位 Field, 屬性 Property, 方法 Method, 事件 Event
    public class ThirdPersonController : MonoBehaviour
    {
        #region Field (欄位)
        // 儲存遊戲資料，例如：移動速度、跳躍高度
        // 常用資料類型：int (整數), float (浮點數), string (字串), bool (布林值)
        // 語法：修飾詞 資料類型 欄位名稱 (指定 預設值) 結尾
        // 修飾詞
        //  public - 允許其他類別存取 - 在屬性面板顯示 - 需要調整的資料設為public
        //  private - 禁止其他類別存取 - 在屬性面板隱藏 - 預設值

        // Attribute (欄位屬性)：輔助欄位資料
        //  語法：[屬性名稱(屬性值)]
        //  Header (標題), Tooltip (提示), Range (數值範圍)
        // ※ Unity運作時屬性面板之設定值優先度高於程式碼
        [Header("移動速度"), Range(0, 500)]
        public float speed = 10.5f;
        [Header("跳躍高度"), Range(0, 1000)]
        public float jump = 100;
        [Header("檢測地面資料")]
        [Tooltip("檢查角色是否在地面上")]
        public bool isGrounded = false;
        public Vector3 checkGroundOffset;
        [Range(0, 3)]
        public float checkGroundRadius = 0.2f;
        [Header("音效")]
        public AudioClip audioJump;
        public AudioClip audioFall;
        [Header("動畫參數")]
        public string animatorParWalk = "WalkingSwitch";
        public string animatorParRun = "RinningSwitch";
        public string animatorParInjury = "InjuryTrigger";
        public string animatorParDeath = "DeathTrigger";
        public string animatorParJump = "JumpTrigger";
        public string animatorParIsGrounded = "IsGrounded";

        [Header("玩家角色物件")]
        public GameObject playerObject;

        private AudioSource aud;
        private Rigidbody rig;
        private Animator aniCtrl;


        #region Unity 資料類型
        /*
        // Color (顏色)
        public Color color;  // 預設值
        public Color red = Color.red;  // 內建顏色
        public Color color1 = new Color(0.5f, 0.5f, 0, 0.5f);  // 自訂顏色，第四欄位 A (透明度) 可省略

        // Vector (座標) 維度 2 - 4
        public Vector2 v2;
        public Vector2 v2Right = Vector2.right;  // 指定特定方向之座標為1
        public Vector3 v3Forward = Vector3.forward;
        public Vector2 v2Custom = new Vector2(7.5f, 100.9f);  // 自訂座標

        // 按鍵 - enum (列舉資料)
        public KeyCode key;
        public KeyCode move = KeyCode.W;
        public KeyCode jump = KeyCode.Space;

        // 遊戲資料類型：不能指定預設值
        public AudioClip sound;  // 音效：支援 mp3, ogg, wav
        public VideoClip video;  // 影片：支援 mp4
        public Sprite sprite;  //圖片：支援 png, jpeg, 不支援 gif
        public Texture2D texture2D;  // 2D 圖片：支援 png, jpeg
        public Material material;  // 材質球

        [Header("元件")]
        // Component (元件) 在Inspector (屬性面板) 上可折疊
        public Transform tra;
        public Animation aniOld;
        public Animator aniNew;
        public Light lig;
        public Camera cam;
        */
        #endregion
        #endregion

        #region Property (屬性)
        #region 屬性練習
        /*
        // 儲存資料，與欄位相同
        // 差異：屬性可以設定存取權限 - Get, Set
        // 語法：修飾詞 資料類型 屬性名稱 { 取; 存; }
        public int ReadAndWrite { get; set; }
        // 唯讀屬性：只能 Get (取得)
        public int Read { get; }
        // 唯讀屬性：透過 Get 設定預設值，關鍵字 Return 為傳回值
        public int ReadValue
        {
            get
            {
                return 77;
            }
        }

        // 唯寫屬性：禁止，必須有 Get
        // public int write { set: }

        private int _HP;
        public int HP
        {
            get
            {
                return _HP;
            }
            set
            {
                _HP = value;
            }
        }
        */
        #endregion

        // C# 7.0 存取子 可以使用 Lambda => 運算子
        private bool keyJump { get => Input.GetKeyDown(KeyCode.Space); }
        #endregion

        #region Method (方法)
        // 定義與實作較複雜程式的區塊，功能
        // 語法：修飾詞 傳回資料類型 方法名稱 (參數1, ... , 參數N) { 程式區塊 }
        // 常用傳回類型：void (無傳回)

        private void Move(float speedMove)
        {
            rig.velocity =
                Vector3.forward * MoveInput("Vertical") * speedMove +
                Vector3.right * MoveInput("Horizontal") * speedMove +
                Vector3.up * rig.velocity.y;
        }
        private float MoveInput(string axisName)
        {
            return Input.GetAxis(axisName);
        }
        private bool CheckGround()
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position +
                transform.right * checkGroundOffset.x +
                transform.up * checkGroundOffset.y +
                transform.forward * checkGroundOffset.z,
                checkGroundRadius, 1 << 3);
            isGrounded = hits.Length > 0;
            return hits.Length > 0;
        }
        private void Jump()
        {
            if (CheckGround() && keyJump) { rig.AddForce(transform.up * jump); }
        }
        private void UpdateAnimation()
        {
            aniCtrl.SetBool(animatorParWalk, MoveInput("Vertical") != 0 || MoveInput("Horizontal") != 0);
            aniCtrl.SetBool(animatorParIsGrounded, isGrounded);
            if (keyJump) aniCtrl.SetTrigger(animatorParJump);
        }

        #region 練習
        /*
        private void Test()
        {
            print("Method Test");
        }

        private int ReturnJump()
        {
            return 999;
        }

        // 參數語法：資料類型 參數名稱 指定 預設值
        // 有預設值的參數可以不輸入引數，選填式參數
        private void Skill(int damage, string effect = "Ash", string sound = "gah")
        {
            print("參數版本 - 傷害值：" + damage);
            print("參數版本 - 技能特效" + effect);
            print("參數版本 - 音效" + sound);
        }
        */
        // 低效率作法
        /*
        private void Skill100()
        {
            print("傷害值：" + 100);
            print("技能特效");
        }
        private void Skill150()
        {
            print("傷害值：" + 150);
            print("技能特效");
        }
        private void Skill200()
        {
            print("傷害值：" + 200);
            print("技能特效");
        }
        */
        #endregion

        #region BMI練習
        /*
        // BMI
        /// <summary>
        /// 計算 BMI 方法
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="height"></param>
        /// <param name="name"></param>
        /// <returns>BMI 結果</returns>
        private float BMI(float weight, float height, string name = "human")
        {
            print(name + "'s BMI");
            return weight / (height * height);
        }
        */
        #endregion

        #endregion

        #region Event (事件)
        // 特定時間點會執行的方法，程式的入口 Start，等於 Console Main
        // 開始事件：遊戲開始時執行一次
        // 處理初始化、抓取資料
        private void Start()
        {
            #region 輸出方法
            /*
            print("Hello MDFK!");

            Debug.Log("一般訊息");
            Debug.LogWarning("警告訊息");
            Debug.LogError("錯誤訊息");
            */
            #endregion

            #region 屬性練習
            /*
            // 欄位與屬性 Get (取得) 與 Set (設定)
            print("欄位資料 - 移動速度：" + MovementSpeed);
            print("屬性資料 - 讀寫屬性：" + ReadAndWrite);
            MovementSpeed = 20.5f;
            ReadAndWrite = 90;
            print("修改後的資料");
            print("欄位資料 - 移動速度：" + MovementSpeed);
            print("屬性資料 - 讀寫屬性：" + ReadAndWrite);
            // 唯讀屬性
            // Read = 7;  // 唯讀屬性不能Set (設定)
            print("唯讀屬性：" + Read);
            print("唯讀屬性，有預設值：" + ReadValue);

            // 屬性存取練習
            print("HP: " + HP);
            HP = 100;
            print("HP: " + HP);
            */
            #endregion

            #region 低效率作法
            /*
            Skill100();
            Skill150();
            Skill200();
            */
            #endregion

            /* BMI 練習
            print(BMI(60, 1.66f, "Coffee"));
            */

            #region 練習
            /*
            // 呼叫有參數方法時，必須輸入對應的引數
            Skill(300);  // 使用選填式參數
            Skill(999, "Explosion");  // 自訂參數
            Skill(500, sound: "shu");  // 有複數選填式參數時可使用指名參數語法：參數名稱: 值


            // 呼叫自訂方法語法：方法名稱();
            Test();
            // 呼叫有傳回值的 Method
            //  1. 區域變數指定傳回值 - 區域變數僅能在此結構內存取
            int j = ReturnJump();
            print("跳躍值：" + j);
            //  2. 將傳回方法當成值使用
            print("跳躍值 (值)：" + (ReturnJump() + 1));
            */
            #endregion

            // 方式1. 物件欄位名稱.取得元件(類型(元件類型)) 當作 元件類型;
            aud = playerObject.GetComponent(typeof(AudioSource)) as AudioSource;
            // 方式2. 此腳本遊戲物件.取得元件<泛型>();
            // gameObject用來指代當前腳本所掛載的遊戲物件本身
            rig = gameObject.GetComponent<Rigidbody>();
            // 方式3. 取得元件<泛型>();
            aniCtrl = GetComponent<Animator>();
        }

        // 更新事件：每秒約執行 60 次 (60 FPS)
        // 處理持續性運動、移動物件、監聽玩家輸入按鍵
        private void Update()
        {
            Jump();
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            Move(speed);
        }

        // 繪製圖示事件：在Unity Editor內繪製圖示輔助開發，發布後會自動隱藏
        private void OnDrawGizmos()
        {
            // 1. 指定顏色；2. 繪製圖形
            Gizmos.color = new Color(1, 0, 0.2f, 0.3f);
            Gizmos.DrawSphere(
                transform.position +
                transform.right * checkGroundOffset.x +
                transform.up * checkGroundOffset.y +
                transform.forward * checkGroundOffset.z,
                checkGroundRadius);
        }
        #endregion
    }
}