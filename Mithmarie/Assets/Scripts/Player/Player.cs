using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Player : StateBehaviour
    {
        [SerializeField] private MouseLook mouseLook = default;
        [SerializeField] private PlayerMovement playerMovement = default;
        [SerializeField] private Terraformer terraformer = default;
        [SerializeField] private Transform eyes = default;

        private PlayerHUD playerHUD;
        private PlayerSettings settings;
        private bool wantsToClose;

        private void Start()
        {
            playerHUD = FindAnyObjectByType<PlayerHUD>();

            string settingsPath = Application.persistentDataPath + "/player_settings.txt";

            print(settingsPath);
            settings = new PlayerSettings();
           // if (File.Exists(settingsPath))
           //     settings = JsonUtility.FromJson<PlayerSettings>(File.ReadAllText(settingsPath));

            ISettingsRequired[] components = GetComponents<ISettingsRequired>();
            components.ToList().ForEach(x => x.AssignSettings(settings));
        }

        public override void OnFrame()
        {
            base.OnFrame();

            playerMovement.OnFrame();
            mouseLook.OnFrame();
            terraformer.OnFrame();

            if (Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame)
                Close();
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

        /*private void OnApplicationQuit()
        {
            string settingsPath = Application.persistentDataPath + "/player_settings.txt";
            settings.version = Application.version;
            File.WriteAllText(settingsPath, JsonUtility.ToJson(settings));
        }*/
    }
}