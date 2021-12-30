using System.Collections;
using UnityEngine;

namespace coffee.Practice
{
    public class LearnCoroutine : MonoBehaviour
    {
        // 定義 Coroutine
        private IEnumerator TestCoroutine()
        {
            print("Start Coroutine");
            yield return new WaitForSeconds(2);
            print("2 Second After Coroutine Started");
        }

        public Transform sphere;
        private IEnumerator SphereScale()
        {
            for (int i = 0; i < 10; i++)
            {
                sphere.localScale += Vector3.one;
                yield return new WaitForSeconds(1);
            }
        }
        // 啟動 Coroutine 需使用指定語法
        private void Start()
        {
            StartCoroutine(TestCoroutine());
            StartCoroutine(SphereScale());
        }
    }
}

