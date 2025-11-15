using SFB;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mitholca
{
    /// <summary>
    /// Handles: saving, loading, closing, settings, fill, circle, wall commands
    /// </summary>
    public class Menu : StateBehaviour
    {
        [SerializeField] private Button saveButton = default;
        [SerializeField] private Button loadButton = default;
        [SerializeField] private Button newButton = default;
        [SerializeField] private Button closeButton = default;

        private World world;
        private bool wantsToClose;

        private void Start()
        {
            world = FindAnyObjectByType<World>();
        }

        private void Save()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Mitholca", "mth") };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            FileStream stream = File.OpenWrite(path);
            BinaryWriter writer = new BinaryWriter(stream);

            world.Flush();
            world.Serialize(writer);

            //writer.Flush();
            //stream.Close();

            // GOING OUT OF SCOPE CLOSES THE STREAM AND WRITER.
            Close();
        }

        private void New()
        {
            world.Clear();
            world.Flush();
            Close();
        }

        private void Load()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "mth", false);
            if (paths.Length <= 0)
                return;

            string path = paths[0];
            FileStream stream = File.OpenRead(path);
            BinaryReader reader = new BinaryReader(stream);

            world.Clear();
            world.Deserialize(reader);
            world.Flush();

            // reader.Close();
            // reader.Close();

            // GOING OUT OF SCOPE CLOSES THE STREAM AND READER.
            Close();
        }

        private void Close() => wantsToClose = true;

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(true);

            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
            newButton.onClick.AddListener(New);

            closeButton.onClick.AddListener(Close);
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);

            wantsToClose = false;

            saveButton.onClick.RemoveListener(Save);
            loadButton.onClick.RemoveListener(Load);
            newButton.onClick.RemoveListener(New);

            closeButton.onClick.RemoveListener(Close);
        }

        public bool GetShouldReturnToPlayer() 
        {
            return Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame || wantsToClose;
        }
    }
}