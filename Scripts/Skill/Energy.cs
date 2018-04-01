using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace MyRPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] GameObject energyBar = null;
        [SerializeField] float maxEnergy = 100;
        [SerializeField] float perConst = 10;

        RawImage energyUI = null;
        float currentEnergy = 0;
        // Use this for initialization
        void Start()
        {
            currentEnergy = maxEnergy;
            energyUI = energyBar.GetComponent<RawImage>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                ShowEneryUI(currentEnergy - perConst);
            }
        }

        void ShowEneryUI(float heath)
        {
            currentEnergy = Mathf.Clamp(heath, 0, maxEnergy);
            float energy = (50 - currentEnergy) * 0.01f;
            energyUI.uvRect = new Rect(energy, 0f, 1, 1);
        }
    }

}
