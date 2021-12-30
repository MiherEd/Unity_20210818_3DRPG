using re.Dialogue;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace re
{
    /// <summary>
    /// 2121.1124
    /// 敵人行為
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region Field Public
        [Header("移動速度"), Range(0, 20)]
        public float speed = 2.5f;
        [Header("攻擊力"), Range(0, 200)]
        public float attack = 35;
        [Header("範圍：追蹤與攻擊")]
        public float rangeAttack = 5;
        public float rangeTrack = 15;
        [Header("攻擊冷卻時間"), Range(0, 5)]
        public float timeAttack = 2.5f;
        [Header("攻擊延遲傳送傷害時間"), Range(0, 5)]
        public float delaySendDamage = 0.5f;

        [Header("等待隨機秒數")]
        public Vector2 v2RandomWait = new Vector2(1f, 5f);
        [Header("走路隨機秒數")]
        public Vector2 v2RandomWalk = new Vector2(3, 7);

        [Header("攻擊區域位移與尺寸")]
        public Vector3 v3AttackOffset;
        public Vector3 v3AttackSize = Vector3.one;

        [Header("面向玩家速度"), Range(0, 50)]
        public float speedLookAt = 5;

        [Header("NPC 名稱")]
        public string nameNPC = "NPCBoy";
        #endregion

        #region Field Private
        [SerializeField]
        private StateEnemy state;
        private bool isIdle;
        private bool isWalk;
        private bool isTrack;
        private bool isAttack;
        private bool targetIsDead;
        private Animator ani;
        private NavMeshAgent nma;
        private string parameterIdleWalk = "WalkingSwitch";
        private string parameterAttack = "AttackTrigger";
        private NPC npc;
        private HurtSystem hurtSystem;
        /// <summary>
        /// 隨機行走座標
        /// </summary>
        private Vector3 v3RandomWalk { get => Random.insideUnitSphere * rangeTrack + transform.position; }
        /// <summary>
        /// 最終隨機行走座標：透過 API 取得網格內可走到的位置
        /// </summary>
        private Vector3 v3RandomWalkFinal;
        /// <summary>
        /// 玩家是否在雷達範圍內
        /// </summary>
        private bool playerInTrackRange { get => Physics.OverlapSphere(transform.position, rangeTrack, 1 << 6).Length > 0; }
        #endregion

        private void OnDrawGizmos()
        {
            #region 攻擊範圍、追蹤範圍與隨機行走座標
            Gizmos.color = new Color(1, 0, 0.2f, 0.3f);
            Gizmos.DrawSphere(transform.position, rangeAttack);

            Gizmos.color = new Color(0.2f, 1, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, rangeTrack);

            if (state == StateEnemy.Walk)
            {
                Gizmos.color = new Color(1, 0, 0.2f, 0.3f);
                Gizmos.DrawSphere(v3RandomWalkFinal, 0.3f);
            }
            #endregion

            #region 攻擊碰撞判定區域（有效傷害範圍）
            Gizmos.color = new Color(0.8f, 0.2f, 0.7f, 0.3f);
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position + transform.right * v3AttackOffset.x +
                transform.up * v3AttackOffset.y +
                transform.forward * v3AttackOffset.z,
                transform.rotation, transform.localScale);
            Gizmos.DrawCube(Vector3.zero, v3AttackSize);
            #endregion
        }

        #region Event
        private Transform traPlayer;
        private string namePlayer = "Brad";

        private void Awake()
        {
            ani = GetComponent<Animator>();
            nma = GetComponent<NavMeshAgent>();
            nma.speed = speed;
            hurtSystem = GetComponent<HurtSystem>();

            traPlayer = GameObject.Find(namePlayer).transform;
            npc = GameObject.Find(nameNPC).GetComponent<NPC>();

            // 受傷系統 - 死亡事件觸發時，呼叫 NPC 更新數量
            // AddListener(Method) 添加監聽器(執行方法)
            hurtSystem.onDead.AddListener(npc.UpdateMissionCount);

            nma.SetDestination(transform.position);
        }

        private void Update()
        {
            StateManager();
        }
        #endregion

        #region Method Private
        /// <summary>
        /// 狀態管理
        /// </summary>
        private void StateManager()
        {
            switch (state)
            {
                case StateEnemy.Idle:
                    Idle();
                    break;
                case StateEnemy.Walk:
                    Walk();
                    break;
                case StateEnemy.Track:
                    Track();
                    break;
                case StateEnemy.Attack:
                    Attack();
                    break;
                case StateEnemy.Hurt:
                    break;
                case StateEnemy.Dead:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 等待：隨機秒數後進入走路狀態
        /// </summary>
        private void Idle()
        {
            if (!targetIsDead && playerInTrackRange) state = StateEnemy.Track;

            if (isIdle) return;
            isIdle = true;

            ani.SetBool(parameterIdleWalk, false);
            StartCoroutine(IdleEffect());
        }

        private IEnumerator IdleEffect()
        {
            float randomWait = Random.Range(v2RandomWait.x, v2RandomWait.y);
            yield return new WaitForSeconds(randomWait);

            state = StateEnemy.Walk;
            isIdle = false;
        }

        /// <summary>
        /// 走路：隨機秒數後進入等待狀態
        /// 掃瞄並儲存行走區域內網格的碰撞資訊後，再隨機抽取可行走的座標為最終座標
        /// </summary>
        private void Walk()
        {
            #region 持續執行區域
            if (!targetIsDead && playerInTrackRange) state = StateEnemy.Track;

            nma.SetDestination(v3RandomWalkFinal);
            ani.SetBool(parameterIdleWalk, nma.remainingDistance > 0.1f);
            #endregion

            if (isWalk) return;
            isWalk = true;

            NavMeshHit hit;
            NavMesh.SamplePosition(v3RandomWalk, out hit, rangeTrack, NavMesh.AllAreas);
            v3RandomWalkFinal = hit.position;

            StartCoroutine(WalkEffect());
        }

        private IEnumerator WalkEffect()
        {
            float randomWalk = Random.Range(v2RandomWalk.x, v2RandomWalk.y);
            yield return new WaitForSeconds(randomWalk);

            state = StateEnemy.Idle;
            isWalk = false;
        }

        /// <summary>
        /// 追蹤玩家
        /// 玩家進入追蹤範圍內時，怪物開始追蹤玩家，進入攻擊範圍內切換至攻擊狀態
        /// </summary>
        private void Track()
        {
            if (!isTrack)
            {
                StopAllCoroutines();
            }
            isTrack = true;

            nma.isStopped = false;
            nma.SetDestination(traPlayer.position);
            ani.SetBool(parameterIdleWalk, true);

            if (nma.remainingDistance <= rangeAttack) state = StateEnemy.Attack;
        }

        /// <summary>
        /// 攻擊玩家
        /// 將玩家座標設為前進目標，進入可攻擊範圍便執行攻擊行為，離開範圍時切換至追蹤狀態
        /// </summary>
        private void Attack()
        {
            nma.isStopped = true;
            ani.SetBool(parameterIdleWalk, false);
            nma.SetDestination(traPlayer.position);
            LookAtPlayer();

            if (nma.remainingDistance > rangeAttack) state = StateEnemy.Track;

            if (isAttack) return;
            isAttack = true;

            ani.SetTrigger(parameterAttack);
            StartCoroutine(DelaySendDamageToTarget());
        }

        /// <summary>
        /// 延遲傳送傷害到目標
        /// 配合物件動畫以符合視覺效果
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelaySendDamageToTarget()
        {
            yield return new WaitForSeconds(delaySendDamage);

            Collider[] hits = Physics.OverlapBox(
                            transform.position + transform.right * v3AttackOffset.x +
                            transform.up * v3AttackOffset.y +
                            transform.forward * v3AttackOffset.z,
                            v3AttackSize / 2, Quaternion.identity, 1 << 6);

            if (hits.Length > 0) targetIsDead = hits[0].GetComponent<HurtSystem>().Hurt(attack);
            if (targetIsDead) TargetDead();

            float waitToNextAttack = timeAttack - delaySendDamage;
            yield return new WaitForSeconds(waitToNextAttack);
            isAttack = false;
        }

        /// <summary>
        /// 目標死亡
        /// </summary>
        private void TargetDead()
        {
            state = StateEnemy.Walk;
            isIdle = false;
            isWalk = false;
            nma.isStopped = false;
        }

        /// <summary>
        /// 使怪物面朝向玩家
        /// </summary>
        private void LookAtPlayer()
        {
            Quaternion angle = Quaternion.LookRotation(traPlayer.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, angle, Time.deltaTime * speedLookAt);
            ani.SetBool(parameterIdleWalk, transform.rotation != angle);
        }
    }
    #endregion
}