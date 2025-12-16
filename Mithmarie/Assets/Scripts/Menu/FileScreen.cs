using SFB;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class FileScreen : Screen
    {
        private string SavePathPath => Application.persistentDataPath + "/lastsave.txt";
        private const float MESSAGE_DURATION = 1f;

        [SerializeField] private Button newButton = default;
        [SerializeField] private Button saveButton = default;
        [SerializeField] private Button saveAsButton = default;
        [SerializeField] private Button loadButton = default;
        [SerializeField] private Button exportButton = default;
        [SerializeField] private Button exportAsChunksButton = default;

        private World world;
        private IBinarySerializable level;
        private IMessageService message;
        private IExportStrategy exportStrat;
        private IWorldMeshStrategy worldMeshStrat;
        private string savePath;
        private string exportPath;

        public void Setup(World world, IExportStrategy exportStrat, IWorldMeshStrategy worldMeshStrat, IBinarySerializable level)
        {
            this.world = world;
            this.exportStrat = exportStrat;
            this.worldMeshStrat = worldMeshStrat;
            this.level = level;

            message = ServiceLocator<IMessageService>.Locate();
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

            message.Send("Saving ...", (Color.green + Color.gray + Color.gray) / 3f, MESSAGE_DURATION);
            File.WriteAllText(SavePathPath, savePath);

            FileStream stream = File.OpenWrite(savePath);
            BinaryWriter writer = new BinaryWriter(stream);

            world.Flush();
            level.Serialize(writer, message);

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
            File.WriteAllText(SavePathPath, savePath);

            Save();
        }

        private void New()
        {
            savePath = string.Empty;
            File.WriteAllText(SavePathPath, savePath);

            world.Clear();
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
            Debug.LogWarning(path);
            if (!Utils.IsStringValid(path) || !File.Exists(path))
                return;

            message.Send(path, Color.gray, MESSAGE_DURATION);
            FileStream stream = File.OpenRead(path);
            BinaryReader reader = new BinaryReader(stream);

            savePath = path;
            File.WriteAllText(SavePathPath, savePath);

            world.Clear();
            level.Deserialize(reader, message);
            reader.Close();

            world.Flush();
            CloseCompletely();
        }

        private void StartExport()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter(exportStrat.GetWholeName(), exportStrat.GetShortName()) };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            message.Send("Exporting ...", Color.gray, MESSAGE_DURATION);
            exportPath = path;

            CancelInvoke(nameof(Export));
            CancelInvoke(nameof(ExportAsChunks));
            Invoke(nameof(Export), 0.1f);
        }

        private void StartExportAsChunks()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter(exportStrat.GetWholeName(), exportStrat.GetShortName()) };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level_chunks", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            message.Send("Exporting Chunks ...", Color.gray, MESSAGE_DURATION);
            exportPath = path;

            CancelInvoke(nameof(ExportAsChunks));
            CancelInvoke(nameof(Export));
            Invoke(nameof(ExportAsChunks), 0.1f);
        }

        private void Export()
        {
            world.Flush();

            Mesh mesh = worldMeshStrat.GenerateMesh(world.GetWorldBlocks());
            exportStrat.Export(exportPath, mesh, message);

            CloseCompletely();
        }

        private void ExportAsChunks()
        {
            world.Flush();

            List<Mesh> meshes = worldMeshStrat.GenerateAsChunks(world.GetChunks());
            exportStrat.ExportAsChunks(exportPath, meshes, message);

            CloseCompletely();
        }

        public override void Enter()
        {
            base.Enter();
            gameObject.SetActive(true);

            saveAsButton.onClick.AddListener(SaveAs);
            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
            exportButton.onClick.AddListener(StartExport);
            exportAsChunksButton.onClick.AddListener(StartExportAsChunks);
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
            exportAsChunksButton.onClick.RemoveListener(StartExportAsChunks);
            exportButton.onClick.RemoveListener(StartExport);
        }
    }
}