using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Characters
{
    public class AudioTrigger : MonoBehaviour
    {

        [SerializeField] AudioClip audioClip = null;
        [SerializeField] float triggerRadius = 5f;

        [SerializeField] bool isOneTimePlay = true;
        bool hasPlayed = false;

        AudioSource audioSource = null;
        SphereCollider sphereCollider = null;


        // Use this for initialization
        void Start()
        {
            InitAudio();
            AddSphereCollider();
        }

        private void InitAudio()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.playOnAwake = false;
        }

        //Add a SphereConllider componment and set it's radius
        void AddSphereCollider()
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = triggerRadius;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                PlayAudio();
        }

        void PlayAudio()
        {
            if (!hasPlayed)
            {
                audioSource.Play();
                if (!isOneTimePlay)
                    hasPlayed = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
        }
    }
}
