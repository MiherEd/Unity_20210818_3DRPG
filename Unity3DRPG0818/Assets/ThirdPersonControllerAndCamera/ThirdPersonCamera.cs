using UnityEngine;

namespace coffee
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        /// <summary>
        /// 2021.1004
        /// 第三人稱攝影機系統
        /// 追蹤指定目標
        /// 可三維旋轉與限制旋轉範圍
        /// </summary>
        #region Field
        [Header("目標物件")]
        public Transform target;
        [Header("追蹤速度"), Range(0, 100)]
        public float speedTrack = 1.5f;
        [Header("水平旋轉速度"), Range(0, 100)]
        public float speedTurnHorizontal = 5;
        [Header("垂直旋轉速度"), Range(0, 100)]
        public float speedTurnVertical = 5;
        [Header("X軸垂直旋轉幅度限制")]
        public Vector2 limitAngleX = new Vector2(-0.2f, 0.2f);
        [Header("攝影機在角色前方垂直旋轉限制")]
        public Vector2 limitAngleFromTarget = new Vector2(-0.2f, 0);

        /// <summary>
        /// 攝影機前方座標與距離
        /// </summary>
        private Vector3 _posForward;
        private float lengthForward = 3;
        #endregion

        #region Property
        /// <summary>
        /// 取得滑鼠水平與垂直座標
        /// </summary>
        private float inputMouseX { get => Input.GetAxis("Mouse X"); }
        private float inputMouseY { get => Input.GetAxis("Mouse Y"); }

        public Vector3 posForward
        {
            get
            {
                _posForward = transform.position + transform.forward * lengthForward;
                _posForward.y = target.position.y;
                return _posForward;
            }
        }
        #endregion

        #region Event
        private void Update()
        {
            TurnCamera();
            LimitAngleXandZFromTarget();
            FreezeAngleZ();
        }
        private void LateUpdate()
        {
            TrackTarget();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0, 1, 0.3f);
            _posForward = transform.position + transform.forward * lengthForward;
            _posForward.y = target.position.y;
            Gizmos.DrawSphere(_posForward, 0.15f);
        }
        #endregion

        #region Method
        /// <summary>
        /// 追蹤目標
        /// </summary>
        private void TrackTarget()
        {
            Vector3 posTarget = target.position;
            Vector3 posCamera = transform.position;
            posCamera = Vector3.Lerp(posCamera, posTarget, speedTrack * Time.deltaTime);
            transform.position = posCamera;
        }
        /// <summary>
        /// 旋轉攝影機
        /// </summary>
        private void TurnCamera()
        {
            transform.Rotate(
                inputMouseY * Time.deltaTime * speedTurnVertical, 
                inputMouseX * Time.deltaTime * speedTurnHorizontal, 0);
        }
        private void LimitAngleXandZFromTarget()
        {
            Quaternion angle = transform.rotation;
            angle.x = Mathf.Clamp(angle.x, limitAngleX.x, limitAngleX.y);
            angle.z = Mathf.Clamp(angle.z, limitAngleFromTarget.x, limitAngleFromTarget.y);
            transform.rotation = angle;
        }
        /// <summary>
        /// 凍結Z軸角度
        /// </summary>
        private void FreezeAngleZ()
        {
            Vector3 angle = transform.eulerAngles;
            angle.z = 0;
            transform.eulerAngles = angle;
        }
        #endregion
    }
}