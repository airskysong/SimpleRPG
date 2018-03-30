using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyRPG.Characters
{
    [RequireComponent(typeof(RawImage))]
    public class EnemyHealthBar : MonoBehaviour
    {
        Enemy _enemy;
        RawImage enmeyUI;

        public Enemy enemy
        {
            set { _enemy = value; }
        }

        // Use this for initialization
        void Start()
        {
            enmeyUI = GetComponent<RawImage>();
            if (_enemy == null)
            {
                Debug.Log("Can't find the enmy in parent object");
            }
            else
            {
                _enemy.OnEnemyHealthChange += OnHealthChange;
            }
        }

        private void LateUpdate()
        {
            Camera cam = Camera.main;
            transform.LookAt(cam.transform);
            transform.rotation = Quaternion.LookRotation(cam.transform.forward);
        }


        void OnHealthChange(int Health)
        {
            float ShowHealth = (50 - Health) * 0.010f;
            enmeyUI.uvRect = new Rect(ShowHealth, 0f, 1, 1);
        }

    }
}
