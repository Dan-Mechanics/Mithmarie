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

        /// <summary>
        /// Should this be handled by game manager???
        /// </summary>
        private PlayerSettings settings;
        private readonly string path =  "/player_settings.txt";

        private void Start()
        {
            world = FindAnyObjectByType<World>();
            message = ServiceLocator<IMessageService>.Locate();
            playerHUD = FindAnyObjectByType<PlayerHUD>();

            settings.Load(Application.persistentDataPath + path, message);
            mouseMovement.GiveSettings(settings);
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

            message.Send("You are inside terrain!", Color.black);
            cooldown = 1.75f;
        }
        
        public bool GetShouldReturnToMenu()
        {
            return Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame;
        }

        public override void Exit()
        {
            base.Exit();
            playerHUD.Hide();
        }

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerHUD.Show();
        }

        private void OnApplicationQuit() => settings.Save(Application.persistentDataPath + path);
    }
}