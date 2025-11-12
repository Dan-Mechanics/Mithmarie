using UnityEngine;

namespace Mitholca
{
    public class Locker : MonoBehaviour
    {
        [SerializeField] private GameObject overlay = default;
        private bool locked;

        /*private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }*/

        /*private void Update()
        {
            *//*if(Input.GetKeyDown(KeyCode.Escape))
                locked = !locked;*//*
            
            overlay.SetActive(!locked);
            Cursor.visible = !locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void OnApplicationFocus(bool focus)
        {
            locked = focus;
        }*/
    }
}
