using SFB;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class FileScreen : Screen
    {
        private string SavePathPath => Application.persistentDataPath + "/lastsave.txt";
        
        public event Action OnDoneWithTask;
        [SerializeField] private Button newButton = default;
        [SerializeField] private Button saveButton = default;
        [SerializeField] private Button saveAsButton = default;
        [SerializeField] private Button loadButton = default;
        [SerializeField] private Button exportButton = default;

        private World world;
        private IMessageService message;
        private IExportStrategy filetype;
        private IMeshingStrategy meshing;
        private string savePath;
        private string exportPath;

        public void Setup(World world, IExportStrategy filetype, IMeshingStrategy meshing, IMessageService message)
        {
            this.world = world;
            this.filetype = filetype;
            this.meshing = meshing;
            this.message = message;

            if (File.Exists(SavePathPath))
                savePath = File.ReadAllText(SavePathPath);

            Load(savePath);
        }

        public void Save()
        {
            if (!Utils.IsStringValid(savePath))
            {
                SaveAs();
                return;
            }

            message.Send("Saving ...", Color.green, 0.5f);
            FileStream stream = File.OpenWrite(savePath);
            BinaryWriter writer = new BinaryWriter(stream);

            world.Flush();
            world.Serialize(writer);

            writer.Flush();
            writer.Close();

            OnDoneWithTask?.Invoke();
        }

        private void SaveAs()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Mithmarie", "mth") };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            savePath = path;
            Save();
        }

        private void New()
        {
            savePath = string.Empty;

            world.Clear();
            world.Flush();
            message.Send("Cleared.", Color.gray, 1f);
            OnDoneWithTask?.Invoke();
        }

        private void Load()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "mth", false);
            if (paths.Length <= 0)
                return;

            string path = paths[0];
            Load(path);
        }

        private void Load(string path)
        {
            if (!Utils.IsStringValid(path) || !File.Exists(path))
                return;

            FileStream stream = File.OpenRead(path);
            BinaryReader reader = new BinaryReader(stream);

            savePath = path;

            world.Clear();
            world.Deserialize(reader);

            reader.Close();

            world.ClearCaches();
            world.Flush();
            message.Send(path, Color.gray, 1f);
            OnDoneWithTask?.Invoke();
        }

        private void StartExporting()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Wavefront", "obj") };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            world.Flush();
            exportPath = path;
            message.Send("Exporting ...", Color.black, 1f);

            CancelInvoke(nameof(Export));
            Invoke(nameof(Export), 0.1f);
        }

        private void Export()
        {
            Mesh mesh = meshing.GenerateMesh(world.GetAllBlocks());
            filetype.Export(exportPath, mesh, message);

            OnDoneWithTask?.Invoke();
        }

        public override void Enter()
        {
            base.Enter();
            gameObject.SetActive(true);

            saveAsButton.onClick.AddListener(SaveAs);
            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
            exportButton.onClick.AddListener(StartExporting);
            newButton.onClick.AddListener(New);
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);

            saveAsButton.onClick.RemoveListener(SaveAs);
            saveButton.onClick.RemoveListener(Save);
            loadButton.onClick.RemoveListener(Load);
            newButton.onClick.RemoveListener(New);
            exportButton.onClick.RemoveListener(StartExporting);
        }

        private void OnApplicationQuit()
        {
            File.WriteAllText(SavePathPath, savePath);
        }
    }
}