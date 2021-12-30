using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace re
{
    /// <summary>
    /// 2021.1124
    /// ���˨t��
    /// �B�z��q�B���˻P���`
    /// </summary>
    public class HurtSystem : MonoBehaviour
    {
        #region Field Public
        [Header("��q"), Range(0, 5000)]
        public float hp = 100;
        [Header("���˨ƥ�")]
        public UnityEvent onHurt;
        [Header("���`�ƥ�")]
        public UnityEvent onDead;
        [Header("�ʵe�ѼơG���˻P���`")]
        public string parameterHurt = "InjuryTrigger";
        public string parameterDead = "DeathTrigger";
        #endregion

        #region Field Private
        // private   �����\�b�l���O�s��
        // public    ���\�Ҧ����O�s��
        // protected �ȭ��l���O�s��
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
        /// ����
        /// </summary>
        /// <param name="damage">�����쪺�ˮ`</param>
        // ���Ϧ����Q�l���O�Ƽg���[�W virtual
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
        /// ���`
        /// </summary>
        private void Dead()
        {
            ani.SetBool(parameterDead, true);
            onDead.Invoke();
        }
        #endregion
    }
}