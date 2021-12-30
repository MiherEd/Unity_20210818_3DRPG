using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace re
{
    /// <summary>
    /// 遊戲管理器
    /// 結束處理
    /// 1. 任務完成
    /// 2. 玩家死亡
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Field
        [Header("群組物件")]
        public CanvasGroup groupFinal;
        [Header("結束畫面標題")]
        public Text textTitle;

        private string titleWin = "You Win";
        private string titleLose = "You Failed...";
        #endregion

        #region Method Public
        public void StartFadeFinalUI(bool win)
        {
            StartCoroutine(FadeFinalUI(win ? titleWin : titleLose));
        }
        #endregion

        #region Method Private
        private IEnumerator FadeFinalUI(string title)
        {
            textTitle.text = title;
            groupFinal.interactable = true;
            groupFinal.blocksRaycasts = true;

            for(int i = 0; i < 10; i++)
            {
                groupFinal.alpha += 0.1f;
                yield return new WaitForSeconds(0.02f);
            }
        }
        #endregion
    }
}