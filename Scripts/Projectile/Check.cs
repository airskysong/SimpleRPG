using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Weapon
{
    public class Check : MonoBehaviour
    {
        int _damage = 10;
        [SerializeField] float DestoryTime = 2;
        public int Damage { get; set; }

        void Start()
        {

            Destroy(gameObject, DestoryTime);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                var check = collision.gameObject.GetComponent<IDamage>();
                check.Damage(_damage);
                Destroy(gameObject, 0.05f);
            }
        }

    }
}
