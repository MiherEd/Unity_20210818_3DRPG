using UnityEngine;
using UnityEngine.Events;

namespace re.Dialogue
{
    /// <summary>
    /// 2021.1021
    /// NPC �t��
    /// �����ؼЬO�_�i�J��ܽd��ö}�ҹ�ܨt��
    /// </summary>
    public class NPC : MonoBehaviour
    {
        #region Field and Property
        [Header("��ܸ��")]
        public DataDialogue dataDialogue;
        [Header("������T")]
        [Range(0, 10)]
        public float checkPlayerRadius;
        public GameObject goTip;
        [Range(0, 10)]
        public float speedLookAt = 3;
        [Header("��ܨt��")]
        public DialogueSystem dialogueSystem;
        [Header("�������Ȩƥ�")]
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
        /// �ˬd���a�O�_�i�J���w�d��
        /// </summary>
        /// <returns>���a�i�J �Ǧ^ true �_�h false</returns>
        private bool CheckPlayer()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, checkPlayerRadius, 1 << 6);
            if (hits.Length > 0) target = hits[0].transform;
            return hits.Length > 0;
        }

        /// <summary>
        /// ���ª��a��V
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
        /// ���a�i�J�d�򤺨åB���U���w���O�A�ǰe��ܨt�ΰ���ШD
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
        /// ��s���ȻݨD�ƶq
        /// ���ȥؼЪ���o��Φ��`��B�z
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