using UnityEngine;
using System;

namespace Mitholca
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject eyesPrefab = default;

        private void Start()
        {
            if (!eyesPrefab)
                throw new Exception($"{nameof(eyesPrefab)} is null !!");
            
            Transform eyes = Instantiate(eyesPrefab, transform).transform;
            eyes.localPosition = Vector3.zero;
            eyes.localRotation = Quaternion.identity;
            eyes.localScale = Vector3.one;

            GetComponent<WorldTerraformer>().Setup(eyes);
            GetComponent<MouseMovement>().Setup(eyes);
        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 2000, 20), $"{Application.dataPath}");
        }
    }
}
