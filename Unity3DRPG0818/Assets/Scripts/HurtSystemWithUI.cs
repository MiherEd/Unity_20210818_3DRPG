using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace re
{
    /// <summary>
    /// 2021.1124
    /// 包含介面的受傷系統
    /// 可以處理血條更新
    /// </summary>
    public class HurtSystemWithUI : HurtSystem
    {
        [Header("要更新的血條")]
        public Image imgHp;

        /// <summary>
        /// 血條效果專用的扣血前血量
        /// </summary>
        private float hpEffectOriginal;

        // 欲複寫父類別成員須使用 override
        public override bool Hurt(float damage)
        {
            hpEffectOriginal = hp;

            // 該成員的父類別基底(base).父類別內的內容
            // 此行代表該父類別的所有內容，若刪除此行則不會執行
            base.Hurt(damage);
            StartCoroutine(HpBarEffect());

            return hp <= 0;
        }

        /// <summary>
        /// 血條遞減效果
        /// </summary>
        private IEnumerator HpBarEffect()
        {
            while (hpEffectOriginal != hp)
            {
                hpEffectOriginal--;
                imgHp.fillAmount = hpEffectOriginal / hpMax;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}