using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {
        private GameObject player;
        Transform cam;
        [SerializeField] float MaxZoom = 2;
        [SerializeField] float MinZoom = 0.8f;
        [SerializeField] float ZoomSpeed = 1;
        [SerializeField] float YawSpeed = 1;
        [SerializeField] float PitchSpeed = 1;
        float Yaw = 0;
        float Pitch = 0;
        float Zoom = 1;

        Vector3 OriginalPosition = Vector3.zero;
        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            cam = Camera.main.transform;
            OriginalPosition = cam.position - transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            float m = Input.GetAxis("Mouse ScrollWheel");
            float Mouse_X = Input.GetAxis("Mouse X");
            float Mouse_Y = Input.GetAxis("Mouse Y");

            if (m != 0)
            {
                Zoom -= m * Time.deltaTime * ZoomSpeed;
                Zoom = Mathf.Clamp(Zoom, MinZoom, MaxZoom);
            }
            if (Input.GetMouseButton(1))
            {
                if (Mouse_X != 0)
                {
                    Yaw += Mouse_X * Time.deltaTime * YawSpeed * 10;
                }
                if (Mouse_Y != 0)
                {
                    Pitch -= Mouse_Y * Time.deltaTime * PitchSpeed * 5;
                    Pitch = Mathf.Clamp(Pitch, -25, 30);
                }
            }

            CameraMove();
        }

        void CameraMove()
        {
            transform.position = player.transform.position;
            cam.position = transform.position + OriginalPosition * Zoom;
            cam.LookAt(transform.position);
            cam.RotateAround(transform.position, transform.up, Yaw);
            cam.RotateAround(transform.position, cam.right, Pitch);
        }
    }
}
