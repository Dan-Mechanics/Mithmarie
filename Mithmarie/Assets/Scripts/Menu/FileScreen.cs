using SFB;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class FileScreen : Screen
    {
        private string SavePathPath => Application.persistentDataPath + "/lastsave.txt";
        private const float STANDARD_MESSAGE_DURATION = 0.5f;

        [SerializeField] private Button newButton = default;
        [SerializeField] private Button saveButton = default;
        [SerializeField] private Button saveAsButton = default;
        [SerializeField] private Button loadButton = default;
        [SerializeField] private Button exportButton = default;

        private World world;
        private IMessageService message;
        private IExportStrategy filetype;
        private IWorldMeshStrategy worldMesh;
        private string savePath;
        private string exportPath;

        public void Setup(World world, IExportStrategy filetype, IWorldMeshStrategy worldMesh, IMessageService message)
        {
            this.world = world;
            this.filetype = filetype;
            this.worldMesh = worldMesh;
            this.message = message;

            if (File.Exists(SavePathPath))
                savePath = File.ReadAllText(SavePathPath);

            LoadPath(savePath);
        }

        public void Save()
        {
            if (!Utils.IsStringValid(savePath))
            {
                SaveAs();
                return;
            }

            message.Send("Saving ...", Color.green, STANDARD_MESSAGE_DURATION);
            FileStream stream = File.OpenWrite(savePath);
            BinaryWriter writer = new BinaryWriter(stream);

            world.Flush();
            world.Serialize(writer);

            writer.Flush();
            writer.Close();

            CloseCompletely();
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
            world.ClearCaches();
            world.Flush();

            CloseCompletely();
        }

        private void Load()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "mth", false);
            if (paths.Length <= 0)
                return;

            LoadPath(paths[0]);
        }

        private void LoadPath(string path)
        {
            if (!Utils.IsStringValid(path) || !File.Exists(path))
                return;

            message.Send(path, Color.gray, STANDARD_MESSAGE_DURATION);
            FileStream stream = File.OpenRead(path);
            BinaryReader reader = new BinaryReader(stream);

            savePath = path;

            world.Clear();
            world.Deserialize(reader);

            reader.Close();

            world.ClearCaches();
            world.Flush();

            CloseCompletely();
        }

        private void StartExporting()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter("Wavefront", "obj") };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            message.Send("Exporting ...", Color.black, STANDARD_MESSAGE_DURATION);
            exportPath = path;

            CancelInvoke(nameof(Export));
            Invoke(nameof(Export), 0.1f);
        }

        private void Export()
        {
            world.Flush();
            Mesh mesh = worldMesh.GenerateMesh(world.GetAllBlocks());
            filetype.Export(exportPath, mesh, message);

            CloseCompletely();
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