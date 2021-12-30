using UnityEngine;

namespace coffee
{
    public class ThirdPersonController : MonoBehaviour
    {
        /// <summary>
        /// 2021.1004
        /// 第三人稱控制器
        /// 移動與跳躍
        /// </summary>
        #region Field
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
        public AudioClip soundJump;
        public AudioClip soundGround;
        [Header("動畫參數")]
        public string animatorParWalk = "走路開關";
        public string animatorParRun = "跑步開關";
        public string animatorParInjury = "受傷觸發";
        public string animatorParDeath = "死亡觸發";
        public string animatorParJump = "跳躍觸發";
        public string animatorParIsGrounded = "地板偵測";

        [Header("玩家角色物件")]
        public GameObject playerObject;

        private AudioSource aud;
        private Rigidbody rig;
        private Animator aniCtrl;
        private ThirdPersonCamera thirdPersonCamera;
        #endregion

        #region Property
        private bool keyJump { get => Input.GetKeyDown(KeyCode.Space); }
        private float volumeRandom { get => Random.Range(0.7f, 1.5f); }
        #endregion

        #region Method
        private void Move(float speedMove)
        {
            rig.velocity =
                transform.forward * MoveInput("Vertical") * speedMove +
                transform.right * MoveInput("Horizontal") * speedMove +
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
            if (!isGrounded && hits.Length > 0) aud.PlayOneShot(soundGround, volumeRandom);
            isGrounded = hits.Length > 0;
            return hits.Length > 0;
        }
        private void Jump()
        {
            if (CheckGround() && keyJump) 
            { 
                rig.AddForce(transform.up * jump);
                aud.PlayOneShot(soundJump, volumeRandom);
            }
        }
        private void UpdateAnimation()
        {
            aniCtrl.SetBool(animatorParWalk, MoveInput("Vertical") != 0 || MoveInput("Horizontal") != 0);
            aniCtrl.SetBool(animatorParIsGrounded, isGrounded);
            if (keyJump) aniCtrl.SetTrigger(animatorParJump);
        }
        [Header("面向速度"), Range(0, 50)]
        public float speedLookAt = 3;
        private void LookAtForward()
        {
            if (MoveInput("Vertical") > 0.1f)
            {
                Quaternion angle = Quaternion.LookRotation(thirdPersonCamera.posForward - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, angle, Time.deltaTime * speedLookAt);
            }
            
        }
        #endregion

        #region Event
        private void Start()
        {
            aud = playerObject.GetComponent(typeof(AudioSource)) as AudioSource;
            rig = gameObject.GetComponent<Rigidbody>();
            aniCtrl = GetComponent<Animator>();

            thirdPersonCamera = FindObjectOfType<ThirdPersonCamera>();
        }
        private void Update()
        {
            Jump();
            UpdateAnimation();
            LookAtForward();
        }

        private void FixedUpdate()
        {
            Move(speed);
        }
        private void OnDrawGizmos()
        {
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
