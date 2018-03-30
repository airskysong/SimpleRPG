using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.Core{
    public class SpinMe : MonoBehaviour
    {

        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;

        void Update()
        {
            float xDegreesPerFrame = xRotationsPerMinute * 360 / 60 * Time.deltaTime; // TODO COMPLETE ME
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = yRotationsPerMinute * 360 / 60 * Time.deltaTime; // TODO COMPLETE ME
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = zRotationsPerMinute * 360 / 60 * Time.deltaTime; // TODO COMPLETE ME
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}

