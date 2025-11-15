using UnityEngine;

namespace Mitholca
{
    public class Player : StateBehaviour
    {
        [SerializeField] private MouseMovement mouseMovement = default;
        [SerializeField] private PlayerMovement playerMovement = default;
        [SerializeField] private WorldEditor worldEditor = default;
        [SerializeField] private Transform eyes = default;

        private World world;
        private IMessageService message;

        private void Start()
        {
            world = FindAnyObjectByType<World>();
            message = ServiceLocator<IMessageService>.Locate();
        }

        public override void OnFrame()
        {
            base.OnFrame();
            mouseMovement.OnFrame();
            playerMovement.OnFrame();
            worldEditor.OnFrame();
        }

        public override void OnTick()
        {
            base.OnTick();

            Vector3Int eyesBlockPos = Utils.ApplyGrid(eyes.position);
            if (world.Has(eyesBlockPos))
                message.Send("You are inside terrain!", Color.black);
        }

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}