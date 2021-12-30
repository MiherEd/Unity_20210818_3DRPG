using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace re
{
    /// <summary>
    /// 2021.1124
    /// 受傷系統
    /// 處理血量、受傷與死亡
    /// </summary>
    public class HurtSystem : MonoBehaviour
    {
        #region Field Public
        [Header("血量"), Range(0, 5000)]
        public float hp = 100;
        [Header("受傷事件")]
        public UnityEvent onHurt;
        [Header("死亡事件")]
        public UnityEvent onDead;
        [Header("動畫參數：受傷與死亡")]
        public string parameterHurt = "InjuryTrigger";
        public string parameterDead = "DeathTrigger";
        #endregion

        #region Field Private
        // private   不允許在子類別存取
        // public    允許所有類別存取
        // protected 僅限子類別存取
        private Animator ani;
        protected float hpMax;
        #endregion

        #region Event
        private void Awake()
        {
            ani = GetComponent<Animator>();
            hpMax = hp;
        }
        #endregion

        #region Method Public
        /// <summary>
        /// 受傷
        /// </summary>
        /// <param name="damage">接受到的傷害</param>
        // 欲使成員被子類別複寫須加上 virtual
        public virtual bool Hurt(float damage)
        {
            if (ani.GetBool(parameterDead)) return true;
            hp -= damage;
            ani.SetTrigger(parameterHurt);
            onHurt.Invoke();
            if (hp <= 0)
            {
                Dead();
                return true;
            }
            else return false;
        }
        #endregion

        #region Method Private
        /// <summary>
        /// 死亡
        /// </summary>
        private void Dead()
        {
            ani.SetBool(parameterDead, true);
            onDead.Invoke();
        }
        #endregion
    }
}