using UnityEngine;

namespace re.Dialogue
{
    /// <summary>
    /// 2021.1021
    /// 對話系統
    /// NPC 對話三階段：接取任務前、任務進行中、完成任務
    /// </summary>
    [CreateAssetMenu(menuName = "coffee/Data Dialogue", fileName = "NPC Data Dialogue")]
    public class DataDialogue : ScriptableObject
    {
        [Header("對話者名稱")]
        public string nameDialogue;
        [Header("任務前對話內容"), TextArea(2, 7)]
        public string[] beforeMission;
        [Header("任務進行中對話內容"), TextArea(2, 7)]
        public string[] missionning;
        [Header("任務完成對話內容"), TextArea(2, 7)]
        public string[] afterMission;
        [Header("任務需求數量"), Range(0, 100)]
        public int countNeed;
        [Header("NPC 任務狀態")]
        public StateNPCMission stateNPCMission = StateNPCMission.BeforeMission;
    }
}