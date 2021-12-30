using UnityEngine;

namespace coffee.Practice
{
    /// <summary>
    /// 認識迴圈
    /// while, do while, for, foreach
    /// </summary>
    public class LearnLoop : MonoBehaviour
    {
        private void Start()
        {
            // while
            // if (bool) { content }    - boolean = true 執行一次
            // while (bool) { content } - boolean = true 持續執行
            int a = 1;
            while (a < 6) { print("while loop: " + a); a++; }

            // for
            // for (default; bool; do) { content }
            for (int i = 0; i < 6; i++) { print("for loop: " + i); }
        }

    }

}
