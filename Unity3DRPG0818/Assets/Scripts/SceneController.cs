using UnityEngine;
using UnityEngine.SceneManagement;

namespace re
{
    /// <summary>
    /// 場景控制
    /// 指定前往某場景
    /// 離開遊戲
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// 載入指定場景
        /// </summary>
        /// <param name="nameScene"></param>
        public void LoadScene(string nameScene)
        {
            SceneManager.LoadScene(nameScene);
        }

        /// <summary>
        /// 離開遊戲
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }
    }
}