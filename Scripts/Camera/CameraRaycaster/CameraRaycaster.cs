using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MyRPG.Characters;

namespace MyRPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D attackCursor = null;
        [SerializeField] Texture2D stiffCursor = null;
        [SerializeField] Vector2 hotspot;

        Texture2D currentCursor = null;

        float maxRaycastDepth = 100f; //Hard coded value

        const int POTENTIALLYWALKABLELAYER = 8;
        const int ENEMYLAYER = 9;

        public delegate void OnMouseOverTerrainHandle(Vector3 position);
        public event OnMouseOverTerrainHandle OnMouseOverPotentiallyWalkable;

        public delegate void OnMouseOverEnemyHandle(Enemy enemy);
        public event OnMouseOverEnemyHandle OnMouseOverEnemy;

        // Update is called once per frame
        void FixedUpdate()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                PerformRayCast(ray);
            }

        }

        void PerformRayCast(Ray ray)
        {
            if (RaycastForEnemy(ray)) { return; }
            if (RaycastForPotentiallyWalkable(ray)) { return; }
            SetCurrentCursor(stiffCursor);
        }

        bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask raycastForEnemy = 1 << ENEMYLAYER;
            bool raycastFroEnemyHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, raycastForEnemy);
            if (raycastFroEnemyHit)
            {
                SetCurrentCursor(attackCursor);
                var enemy = hitInfo.collider.gameObject.GetComponent<Enemy>();
                OnMouseOverEnemy(enemy);
                return true;
            }
            return false;
        }

        bool RaycastForPotentiallyWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLYWALKABLELAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit)
            {
                SetCurrentCursor(walkCursor);
                OnMouseOverPotentiallyWalkable(hitInfo.point); // nodifiy to PlayerMovement
                return true;
            }
            return false;
        }

        void SetCurrentCursor(Texture2D targetCursor)
        {
            if (currentCursor != targetCursor)
            {
                currentCursor = targetCursor;
                Cursor.SetCursor(currentCursor, hotspot, CursorMode.Auto);
            }
        }
    }
}

