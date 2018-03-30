using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

using MyRPG.Weapon;

namespace MyRPG.Characters
{
    public class Enemy : MonoBehaviour, IDamage
    {

        [SerializeField] int Health = 100;
        [Tooltip("Require a Canvas name of EnemyUI")]
        [SerializeField]
        GameObject EnemyUI = null;
        [Tooltip("Require a Empty as the povit to instantiate the HealthBarUI")]
        [SerializeField]
        GameObject EnemyUISlot = null;
        [SerializeField] GameObject SpawnBullet = null;
        [SerializeField] float DefenceRadius = 20f;
        [SerializeField] float Stopdistance = 2f;
        [SerializeField] int attackDamage = 2;
        [SerializeField] float attackInterval = 2.0f;
        [SerializeField] float shootSpeed = 1.5f;
        [SerializeField] float attackRange = 5.0f;
        [SerializeField] Weapons Weapon = null;
        //[SerializeField] bool ShrowEnable = false;

        GameObject SpawnBulletPoint = null;

        GameObject enemyUI = null;
        NavMeshAgent nav;
        GameObject Player;
        Animator animator;
        AnimatorOverrideController animatorOverrideController;

        ThirdPersonCharacter character;
        bool AttackEnable = true;
        bool isChase = false;
        Vector3 originalPosition;

        public delegate void OnEnemyHealthChangeHandle(int Health);
        public event OnEnemyHealthChangeHandle OnEnemyHealthChange; // Notify other observes if the health of enemy changed

        void Start()
        {
            if (EnemyUI != null && EnemyUISlot != null)
            {
                enemyUI = Instantiate(EnemyUI, EnemyUISlot.transform);
                enemyUI.transform.localPosition = Vector3.zero;  //Reset the location of HealthUI
                EnemyHealthBar healthbar = enemyUI.GetComponentInChildren<EnemyHealthBar>();
                healthbar.enemy = this;
                enemyUI.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
                nav = GetComponent<NavMeshAgent>();
                nav.stoppingDistance = Stopdistance;
                nav.updatePosition = true;

                originalPosition = transform.position;
                character = GetComponent<ThirdPersonCharacter>();
                Player = GameObject.FindGameObjectWithTag("Player");
                animator = GetComponent<Animator>();
                animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                animator.runtimeAnimatorController = animatorOverrideController;
                UpdateAnimation();
            }
            else
            {
                Debug.Log("Can't find EnemyUI or EnemyUISlot, Please attach them");
            }
        }
        public void Damage(int damage)
        {
            Health = Mathf.Clamp(Health - damage, 0, 100);
            OnEnemyHealthChange(Health);
        }

        void UpdateAnimation()
        {
            if (Weapon != null)
            {

            }
            else
                UpdateWeapon(Weapon.GetObject());
            AnimationClip clip = Weapon.GetAnimateClip();
            UpdateAction(clip);
        }

        void UpdateWeapon(GameObject weapon)
        {
            Transform weaponSpawnPoint = GetComponentInChildren<HandMark>().transform;
            GameObject g = Instantiate(weapon, weaponSpawnPoint);
            g.transform.localRotation = Weapon.GetTrans().rotation;
            g.transform.localPosition = Weapon.GetTrans().position;
        }

        void UpdateAction(AnimationClip clip)
        {
            animatorOverrideController["Default Attack"] = clip;
            //clip = RemoveAnimationClipEvent(clip);
            animator.SetFloat("AttackSpeed", clip.length / attackInterval); //Chang Enemy Attack Interval
        }

        AnimationClip RemoveAnimationClipEvent(AnimationClip clip)
        {
            clip.events = new AnimationEvent[0];
            return clip;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            CheckPlayer();
        }

        // Whether to attack or move is determined by the player's location
        void CheckPlayer()
        {
            Vector3 target = Player.transform.position;
            float distance = Vector3.Distance(transform.position, target);
            if (distance <= DefenceRadius)
            {
                EnemyMove(target);
                isChase = true;
                if (distance <= attackRange && AttackEnable)
                {
                    AttackEnable = false;
                    StartCoroutine(EnemyAttack());
                }
            }
            else
            {
                isChase = false;
                EnemyMove(originalPosition);
            }

        }

        IEnumerator EnemyAttack()
        {
            animator.SetTrigger("Attack");
                        animator.SetFloat("Forward", 0);
            animator.SetFloat("Turn", 0);
            yield return new WaitForSeconds(attackInterval);
            AttackEnable = true;
        }

        void Shoot()
        {
            ThrowBall();
        }

        void ThrowBall()
        {
            Vector3 targetPosition = Player.transform.position + new Vector3(0, .8f, 0);
            Transform SpawnTransform = GetComponentInChildren<HandMark>().transform;      


            if(SpawnBulletPoint==null)
            {
                SpawnBulletPoint = Instantiate(new GameObject("SpawnBulletPoint"), SpawnTransform.position, Quaternion.identity);
                SpawnBulletPoint.transform.SetParent(transform);
            }

            GameObject bullet = Instantiate(SpawnBullet, SpawnBulletPoint.transform.position, Quaternion.identity);
            bullet.transform.SetParent(SpawnBulletPoint.transform);


            bullet.GetComponent<Check>().Damage = attackDamage;
            Vector3 shootDirection = (targetPosition - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = shootDirection * shootSpeed;
        }

        void EnemyMove(Vector3 target)
        {
            nav.SetDestination(target);
            if (nav.remainingDistance > nav.stoppingDistance)
                character.Move(nav.desiredVelocity, false, false);
            else
            {
                character.Move(Vector3.zero, false, false);
                transform.LookAt(target);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DefenceRadius); // Draw distance of Defence
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange); // Draw distance of AttackRang
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Stopdistance); // Draw distance of Stopdistance;
            if (isChase)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(transform.position, Player.transform.position);
            }
        }
    }
}
