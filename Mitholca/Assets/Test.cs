using UnityEngine;
using TMPro;

namespace Mitholca
{
    public class Test : MonoBehaviour
    {
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        private void FixedUpdate()
        {
            text.text = Screen.currentResolution.ToString();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
