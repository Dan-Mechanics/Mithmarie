using SFB;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class FilePage : StateBehaviour
    {
        public event Action OnDone;
        
        [SerializeField] private Button newButton = default;
        [SerializeField] private Button saveButton = default;
        [SerializeField] private Button loadButton = default;
        [SerializeField] private Button exportButton = default;

        private World world;
        private IMessageService message;
        private IExportStrategy exportStrat;
        private IMeshingStrategy generatable;

        public void Setup(World world, IExportStrategy exportStrat, IMeshingStrategy generatable, IMessageService message)
        {
            this.world = world;
            this.exportStrat = exportStrat;
            this.generatable = generatable;
            this.message = message;
        }

        private void Save()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Mithmarie", "mth") };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            FileStream stream = File.OpenWrite(path);
            BinaryWriter writer = new BinaryWriter(stream);

            world.Flush();
            world.Serialize(writer);

            OnDone?.Invoke();
        }

        private void New()
        {
            world.Clear();
            world.Flush();
            OnDone?.Invoke();
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

            OnDone?.Invoke();
        }

        private void Export()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Wavefront", "obj") };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            world.Flush();

            Mesh mesh = generatable.GenerateMesh(world.GetAllBlocks());
            exportStrat.Export(path, mesh, message);

            OnDone?.Invoke();
        }

        public override void Enter()
        {
            base.Enter();
            gameObject.SetActive(true);

            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
            exportButton.onClick.AddListener(Export);
            newButton.onClick.AddListener(New);
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);

            saveButton.onClick.RemoveListener(Save);
            loadButton.onClick.RemoveListener(Load);
            newButton.onClick.RemoveListener(New);
            exportButton.onClick.RemoveListener(Export);
        }
    }
}