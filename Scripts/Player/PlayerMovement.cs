using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using MyRPG.CameraUI;


namespace MyRPG.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovement : MonoBehaviour
    {
        bool isDirectMovement = false;
        ThirdPersonCharacter character = null;
        CameraRaycaster raycaseter = null;
        NavMeshAgent nav = null;

        Vector3 destination;
        float stopDistance = 0;
        Vector3 gizmoDest, gizmoRecentD;

        const int Walkable = 8;
        const int Enemy = 9;

        bool isMoveToEnemy = false;
        Transform enemyTranform = null;

        void Start()
        {
            character = GetComponent<ThirdPersonCharacter>();
            raycaseter = Camera.main.GetComponentInParent<CameraRaycaster>();
            nav = GetComponent<NavMeshAgent>();

            stopDistance = nav.stoppingDistance;
            nav.updateRotation = false;
            nav.updatePosition = true;

            destination = transform.position;
            gizmoDest = gizmoRecentD = destination;

            if (raycaseter != null)
            {
                raycaseter.notifyMouseClickObservers += OnMouseClick; // observer
            }
        }

        void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                print("G is pressed!");
                isDirectMovement = !isDirectMovement; //Toggle mode 
                nav.isStopped = isDirectMovement;
                destination = transform.position; // Reset the targetPosition         
            }
        }
        void FixedUpdate()
        {
            if (isDirectMovement)
            {
                ProcessDirectMovement();
            }
            else
            {
                ProcessMouseMovement();
            }
        }
        private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 Cam_forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 Movement = v * Cam_forward + h * Vector3.right;
            character.Move(Movement, false, false);
        }

        void OnMouseClick(RaycastHit raycastHit, int layer)
        {
            if (!isDirectMovement)
            {
                switch (layer)
                {
                    case Walkable:
                        isMoveToEnemy = false;
                        nav.stoppingDistance = stopDistance;
                        destination = raycastHit.point;
                        break;
                    case Enemy:
                        isMoveToEnemy = true;
                        enemyTranform = raycastHit.collider.transform;
                        break;
                    default:
                        destination = transform.position;
                        break;
                }
            }
        }

        void ProcessMouseMovement()
        {
            gizmoDest = destination;
            Vector3 ClickPoint = destination - transform.position;
            gizmoRecentD = (ClickPoint.magnitude - nav.stoppingDistance) * ClickPoint.normalized + transform.position;
            if (!nav.enabled)
            {
                destination = enemyTranform.position;
                return;
            }
            if (!isMoveToEnemy)
                CharacterMoveByAI(destination);
            else
            {
                CharacterMoveByAI(enemyTranform.transform.position);
            }
        }

        void CharacterMoveByAI(Vector3 target)
        {
            nav.SetDestination(target);
            if (nav.remainingDistance > nav.stoppingDistance)
            {
                character.Move(nav.desiredVelocity, false, false);
            }
            else
                character.Move(Vector3.zero, false, false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, gizmoDest);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gizmoDest, 0.2f); //draw the sphere of destination
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(gizmoRecentD, 0.2f);
        }


    }
}
