using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Weapon
{
    [CreateAssetMenu(menuName = ("MyRPG/Weapons"))]
    public class Weapons : ScriptableObject
    {
        [SerializeField] GameObject Weapon = null;
        [SerializeField] Transform Spwan = null;
        [SerializeField] AnimationClip animationClip = null;

        public GameObject GetObject()
        {
            return Weapon;
        }
        public Transform GetTrans()
        {
            return Spwan;
        }
        public AnimationClip GetAnimateClip()
        {
            return animationClip;
        }
    }
}
