using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Player : StateBehaviour
    {
        [SerializeField] private Transform eyes = default;
        private StateBehaviour[] playerBehaviour;

        private PlayerHUD playerHUD;
        private bool wantsToClose;

        private void Start()
        {
            playerHUD = FindAnyObjectByType<PlayerHUD>();

            List<StateBehaviour> list = GetComponents<StateBehaviour>().ToList();
            list.RemoveAt(list.FindIndex(x => x is Player));
            playerBehaviour = list.ToArray();
        }

        public override void OnFrame()
        {
            base.OnFrame();

            for (int i = 0; i < playerBehaviour.Length; i++)
            {
                playerBehaviour[i].OnFrame();
            }

            if (Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame)
                Close();
        }

        public override void OnTick()
        {
            base.OnTick();
            for (int i = 0; i < playerBehaviour.Length; i++)
            {
                playerBehaviour[i].OnTick();
            }
        }

        public bool GetWantsToClose() => wantsToClose;
        private void Close() => wantsToClose = true;

        public override void Exit()
        {
            base.Exit();
            wantsToClose = false;

            if (playerHUD != null)
                playerHUD.Hide();
        }

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerHUD.Show();
        }
    }
}