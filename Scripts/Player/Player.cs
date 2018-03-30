using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using MyRPG.CameraUI;
using MyRPG.Weapon;

namespace MyRPG.Characters
{
    public class Player : MonoBehaviour, IDamage
    {
        [SerializeField] int MaxHealth = 100;
        GameObject Target = null;
        CameraRaycaster raycaseter = null;
        Animator animator;
        AnimatorOverrideController animatorOverrideController;

        [SerializeField] int AttackDamage = 10;
        [SerializeField] int AttackRange = 2;
        [SerializeField] int AttackInterVal = 1;
        [SerializeField] Weapons weapon = null;

        NavMeshAgent nav = null;


        const int Enemy = 9;

        bool enableAttack = true;
        int Health = 0;

        public delegate void OnHealthChangeHandle(int Health);
        public event OnHealthChangeHandle OnHealthChange;

        void Start()
        {
            Health = MaxHealth;
            raycaseter = Camera.main.GetComponentInParent<CameraRaycaster>();
            if (raycaseter != null)
            {
                raycaseter.notifyMouseClickObservers += OnMouseClick; // observer
            }
            nav = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;
            UpdateAniamtion(weapon);
        }

        void FixedUpdate()
        {

        }

        //Consider to reference When Player get hurt;
        public void Damage(int damage)
        {
            Health = Mathf.Clamp(Health - damage, 0, 100);
            OnHealthChange(Health);
        }

        void UpdateAniamtion(Weapons weapon)
        {
            UpdateWeaponModel();
            AnimationClip clip = weapon.GetAnimateClip();
            UpdateAnimationClip(clip);
        }

        void UpdateWeaponModel()
        {
            GameObject equipment = weapon.GetObject();
            Transform Spawn = weapon.GetTrans();
            Transform weaponSlot = GetComponentInChildren<HandMark>().transform;
            GameObject g = Instantiate(equipment, weaponSlot);
            g.transform.localPosition = Spawn.localPosition;
            g.transform.localRotation = Spawn.localRotation;
        }

        void UpdateAnimationClip(AnimationClip clip)
        {
            animatorOverrideController["Default Attack"] = clip;
            clip = RemoveAnimationEvent(clip);
            animator.SetFloat("AttackSpeed", clip.length / AttackInterVal);
            
        }
        //Consider to Remove AnimationEvent if it existed;
        AnimationClip RemoveAnimationEvent(AnimationClip clip)
        {
            clip.events = new AnimationEvent[0];
            return clip;
        }

        //CallBack when click to enemy;
        void OnMouseClick(RaycastHit hit, int layer)
        {
            if (layer == Enemy)
            {
                Target = hit.collider.gameObject;
                CheckEnemy();
            }
        }

        void CheckEnemy()
        {
            float distance = Vector3.Distance(transform.position, Target.transform.position);
            if (distance <= AttackRange && enableAttack)
            {
                nav.stoppingDistance = AttackRange;
                enableAttack = false;
                transform.LookAt(Target.transform.position);
                StartCoroutine(AttackEnemy());
            }
        }
        // Contol the speed of Attacking;
        IEnumerator AttackEnemy()
        {
            nav.enabled = false;
            animator.SetTrigger("Attack");
            animator.SetFloat("Forward", 0);
            animator.SetFloat("Turn", 0);
            yield return new WaitForSeconds(0.3f);
            Target.GetComponent<IDamage>().Damage(AttackDamage);
            yield return new WaitForSeconds(AttackInterVal);
            enableAttack = true;
            nav.enabled = true;
        }



        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            if (Target != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, Target.transform.position);
            }
        }
    }
}
