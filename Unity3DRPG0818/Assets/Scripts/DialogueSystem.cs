using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace re.Dialogue
{
    /// <summary>
    /// 2021.1021
    /// 對話系統
    /// 顯示對話框與文字浮現效果
    /// </summary>
    public class DialogueSystem : MonoBehaviour
    {
        [Header("對話系統介面物件")]
        public CanvasGroup groupDialogue;
        public Text textName;
        public Text textContent;
        public GameObject goTriangle;
        [Header("對話間隔"), Range(0, 10)]
        public float dialogueInterval = 0.3f;
        [Header("對話按鍵")]
        public KeyCode dialogueKey = KeyCode.Space;
        [Header("打字事件")]
        public UnityEvent onType;

        /// <summary>
        /// 開始對話
        /// </summary>
        public void Dialogue(DataDialogue data)
        {
            StopAllCoroutines();
            StartCoroutine(SwitchDialogueGroup());
            StartCoroutine(ShowDialogueContent(data));
        }

        /// <summary>
        /// 停止對話
        /// </summary>
        public void StopDialogue()
        {
            StopAllCoroutines();
            StartCoroutine(SwitchDialogueGroup(false));
        }

        /// <summary>
        /// 切換對話框群組
        /// </summary>
        /// <returns></returns>
        private IEnumerator SwitchDialogueGroup(bool fadeIn = true)
        {
            float increase = fadeIn ? 0.1f : -0.1f;

            for (int i = 0; i < 10; i++)
            {
                groupDialogue.alpha += increase;
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// 顯示對話內容
        /// </summary>
        /// <param name="data">對話資料</param>
        /// <returns></returns>
        private IEnumerator ShowDialogueContent(DataDialogue data)
        {
            textName.text = "";
            textName.text = data.nameDialogue;

            string[] dialogueContents = { };

            switch (data.stateNPCMission)
            {
                case StateNPCMission.BeforeMission:
                    dialogueContents = data.beforeMission;
                    break;
                case StateNPCMission.Missionning:
                    dialogueContents = data.missionning;
                    break;
                case StateNPCMission.AferMission:
                    dialogueContents = data.afterMission;
                    break;
            }

            for (int j = 0; j < dialogueContents.Length; j++)
            {
                textContent.text = "";
                goTriangle.SetActive(false);

                for (int i = 0; i < dialogueContents[j].Length; i++)
                {
                    textContent.text += dialogueContents[j][i];
                    yield return new WaitForSeconds(dialogueInterval);
                }

                goTriangle.SetActive(true);
                while (!Input.GetKeyDown(dialogueKey)) yield return null;
            }

            StartCoroutine(SwitchDialogueGroup(false));
        }
    }
}