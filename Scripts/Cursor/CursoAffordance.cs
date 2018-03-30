using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyRPG.CameraUI
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursoAffordance : MonoBehaviour
    {
        CameraRaycaster Raycaster;
        [SerializeField] Texture2D[] cursorTexture;
        [SerializeField] Vector2 hotspot;
        const int Walkable = 8;
        const int Enemy = 9;
        const int Stiff = 10;
        // Use this for initialization
        void Start()
        {
            Raycaster = GetComponent<CameraRaycaster>();
            if (Raycaster != null)
            {
                Raycaster.notifyLayerChangeObservers += OnLayerChanged;
            }
        }

        void OnLayerChanged(int layer)
        {
            switch (layer)
            {
                case Walkable:
                    Cursor.SetCursor(cursorTexture[0], hotspot, CursorMode.Auto);
                    break;
                case Enemy:
                    Cursor.SetCursor(cursorTexture[1], hotspot, CursorMode.Auto);
                    break;
                case Stiff:
                    Cursor.SetCursor(cursorTexture[2], hotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(null, hotspot, CursorMode.Auto);
                    return;
            }
        }
    }
}
