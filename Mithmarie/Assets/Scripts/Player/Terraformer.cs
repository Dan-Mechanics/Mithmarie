using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Terraformer : StateBehaviour, ISettingsRequired
    {
        public const Key SELECTION_KEY = Key.LeftAlt;
        
        public Raycast AddRaycast => addRaycast;
        public Raycast RemoveRaycast => removeRaycast;

        public event Action OnAnyAction;
        public event Action OnAdd;
        public event Action OnRemove;
        // TODO: FILL ADD AND REMOVE.

        [SerializeField] private Transform eyes = default;
        [SerializeField] private Raycast addRaycast = default;
        [SerializeField] private Raycast removeRaycast = default;

        private World world;
        private PlayerSettings settings;

        private bool Place => settings.leftClickIsDestroy ? Mouse.current.rightButton.wasPressedThisFrame : Mouse.current.leftButton.wasPressedThisFrame;
        private bool Remove => settings.leftClickIsDestroy ? Mouse.current.leftButton.wasPressedThisFrame : Mouse.current.rightButton.wasPressedThisFrame;

        private void Start()
        {
            world = FindAnyObjectByType<World>();
        }

        public override void OnFrame()
        {
            base.OnFrame();

            if (Keyboard.current[Key.G].wasPressedThisFrame)
                world.DebugDobule();

            // ADD. ===
            if (Place)
            {
                OnAdd?.Invoke();
                OnAnyAction?.Invoke();

                if(addRaycast.Cast(eyes, out RaycastHit hit))
                {
                    world.Add(Utils.ApplyGrid(hit.point + (hit.normal * 0.4f)));
                    world.Flush();
                }
            }

            // REMOVE. ===
            if (Remove)
            {
                OnRemove?.Invoke();
                OnAnyAction?.Invoke();

                if (removeRaycast.Cast(eyes, out RaycastHit hit))
                {
                    world.Remove(Utils.ApplyGrid(hit.point + (hit.normal * -0.4f)));
                    world.Flush();
                }
            }
        }

        public void AssignSettings(PlayerSettings settings) => this.settings = settings;
    }
}
