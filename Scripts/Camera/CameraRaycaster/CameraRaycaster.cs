using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyRPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        //INSPECTOR PRORERITIES RENDERED BY CUSTOM EDITOR SCRIT
        [SerializeField] int[] layerPriorities;

        float maxRaycastDepth = 100f; //Hard coded value
        int topPriorityLayerLastFrame = -1;

        //Setup delegate for broadcasting layer changes to toher classes
        public delegate void OnCursorLayerChangedHandle(int newlayer); // declare new delegate type
        public event OnCursorLayerChangedHandle notifyLayerChangeObservers;// instantiate an observer set

        public delegate void OnClickPriorityLayerHandle(RaycastHit hit, int layer);// declare new delegate type
        public event OnClickPriorityLayerHandle notifyMouseClickObservers;// instantiate an observer set


        // Update is called once per frame
        void FixedUpdate()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                NotifyObserversIfLayerChanged(5);
                return;//Stop looking for other objects
            }

            //Raycast to max depth, every frame as things can move under mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);
            RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);

            if (!priorityHit.HasValue) //if hit no priority object
            {
                NotifyObserversIfLayerChanged(0); // broadcast default layer
                return;
            }

            //notify delegates of layer changes
            var layerHit = priorityHit.Value.collider.gameObject.layer;
            NotifyObserversIfLayerChanged(layerHit);

            //notify delegate of highest priority game object under mouse when clicked
            if (Input.GetMouseButton(0))
            {
                notifyMouseClickObservers(priorityHit.Value, layerHit);
            }

        }

        void NotifyObserversIfLayerChanged(int newLayer)
        {

            if (newLayer != topPriorityLayerLastFrame)
            {
                topPriorityLayerLastFrame = newLayer;
                notifyLayerChangeObservers(newLayer);
            }
        }


        RaycastHit? FindTopPriorityHit(RaycastHit[] raycastHit)
        {
            ////From list of layer numbers hit
            //List<int> layerOfHitCollections = new List<int>();
            //foreach(RaycastHit hit in raycastHit)
            //{
            //    layerOfHitCollections.Add(hit.collider.gameObject.layer);
            //}

            // step through layers in order of priority looking for a gameobject with that layer
            foreach (int layer in layerPriorities)
            {
                foreach (RaycastHit hit in raycastHit)
                {
                    if (hit.collider.gameObject.layer == layer)
                    {
                        return hit;// stop looking
                    }
                }
            }
            return null; // because cannot use Gameobject?nullable
        }

    }
}
