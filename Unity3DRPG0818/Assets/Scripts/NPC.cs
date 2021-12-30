using UnityEngine;
using UnityEngine.Events;

namespace re.Dialogue
{
    /// <summary>
    /// 2021.1021
    /// NPC 系統
    /// 偵測目標是否進入對話範圍並開啟對話系統
    /// </summary>
    public class NPC : MonoBehaviour
    {
        #region Field and Property
        [Header("對話資料")]
        public DataDialogue dataDialogue;
        [Header("相關資訊")]
        [Range(0, 10)]
        public float checkPlayerRadius;
        public GameObject goTip;
        [Range(0, 10)]
        public float speedLookAt = 3;
        [Header("對話系統")]
        public DialogueSystem dialogueSystem;
        [Header("完成任務事件")]
        public UnityEvent onFinish;

        private Transform target;
        private bool startDialogueKey { get => Input.GetKeyDown(KeyCode.E); }
        private int countCurrent;
        #endregion
        
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0.2f, 0.3f);
            Gizmos.DrawSphere(transform.position, checkPlayerRadius);
        }

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            goTip.SetActive(CheckPlayer());
            LookAtPlayer();
            StartDialogue();
        }

        private void Initialize()
        {
            dataDialogue.stateNPCMission = StateNPCMission.BeforeMission;
        }

        /// <summary>
        /// 檢查玩家是否進入指定範圍
        /// </summary>
        /// <returns>玩家進入 傳回 true 否則 false</returns>
        private bool CheckPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, checkPlayerRadius, 1 << 6);
            if (hits.Length > 0) target = hits[0].transform;
            return hits.Length > 0;
        }

        /// <summary>
        /// 面朝玩家轉向
        /// </summary>
        private void LookAtPlayer()
        {
            if (CheckPlayer())
            {
                Quaternion angle = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, angle, Time.deltaTime * speedLookAt);
            }
        }

        /// <summary>
        /// 玩家進入範圍內並且按下指定指令，傳送對話系統執行請求
        /// </summary>
        private void StartDialogue()
        {
            if (CheckPlayer() && startDialogueKey)
            {
                dialogueSystem.Dialogue(dataDialogue);

                if (dataDialogue.stateNPCMission == StateNPCMission.BeforeMission)
                    dataDialogue.stateNPCMission = StateNPCMission.Missionning;
            }
            else if (!CheckPlayer()) dialogueSystem.StopDialogue();
        }

        /// <summary>
        /// 更新任務需求數量
        /// 任務目標物件得到或死亡後處理
        /// </summary>
        public void UpdateMissionCount()
        {
            countCurrent++;

            if (countCurrent == dataDialogue.countNeed)
            {
                dataDialogue.stateNPCMission = StateNPCMission.AferMission;
                onFinish.Invoke();
            }
        }
    }
}