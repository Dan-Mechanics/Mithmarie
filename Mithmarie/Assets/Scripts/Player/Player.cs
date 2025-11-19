using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Player : StateBehaviour
    {
        [SerializeField] private MouseMovement mouseMovement = default;
        [SerializeField] private PlayerMovement playerMovement = default;
        [SerializeField] private WorldEditor worldEditor = default;
        [SerializeField] private Transform eyes = default;

        private World world;
        private IMessageService message;
        private PlayerHUD playerHUD;
        private float cooldown;
        private PlayerSettings settings;

        private void Start()
        {
            world = FindAnyObjectByType<World>();
            message = ServiceLocator<IMessageService>.Locate();
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
            mouseMovement.OnFrame();
            worldEditor.OnFrame();
        }

        public override void OnTick()
        {
            base.OnTick();

            // !TIMER
            cooldown -= Time.fixedDeltaTime;
            if (cooldown > 0f)
                return;

            Vector3Int eyesBlockPos = Utils.ApplyGrid(eyes.position);
            if (!world.Has(eyesBlockPos))
                return;

            message.Send("You are inside a block.", Color.gray);
            cooldown = 5f;
        }
        
        public bool GetShouldReturnToMenu()
        {
            return Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame;
        }

        public override void Exit()
        {
            base.Exit();
            playerHUD?.Hide();
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