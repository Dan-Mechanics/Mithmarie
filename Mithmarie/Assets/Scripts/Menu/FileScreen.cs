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
        private bool unsavedChanges;

        public void Setup(World world, IExportStrategy exportStrat, IWorldMeshStrategy worldMeshStrat, IBinarySerializable level)
        {
            this.world = world;
            this.exportStrat = exportStrat;
            this.worldMeshStrat = worldMeshStrat;
            this.level = level;

            world.OnNewChanges += OnNewChanges;
            message = ServiceLocator<IMessageService>.Locate();

            if (File.Exists(SavePathPath))
                savePath = File.ReadAllText(SavePathPath);

            LoadPath(savePath);
        }

        private void OnNewChanges(HashSet<Vector3Int> added, HashSet<Vector3Int> removed) => unsavedChanges = true;

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

            unsavedChanges = false;

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
            if (CheckUnsavedChanges())
                return;

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

        private bool CheckUnsavedChanges()
        {
            if (!unsavedChanges)
                return false;
            
            message.Send("You have unsaved changes!\nTry again to confirm.", Color.red, 3.0f);
            unsavedChanges = false;

            return true;
        }

        private void LoadPath(string path)
        {
            if (!Utils.IsStringValid(path) || !File.Exists(path))
                return;

            if (CheckUnsavedChanges())
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

        private void Export()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter(exportStrat.GetWholeName(), exportStrat.GetShortName()) };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            message.Send("Exporting ...", Color.gray, MESSAGE_DURATION);
            world.Flush();

            Mesh mesh = worldMeshStrat.GenerateMesh(world.GetWorldBlocks());
            exportStrat.Export(path, mesh, message);

            CloseCompletely();
        }

        private void ExportAsChunks()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter(exportStrat.GetWholeName(), exportStrat.GetShortName()) };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level_chunks", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            message.Send("Exporting Chunks ...", Color.gray, MESSAGE_DURATION);
            world.Flush();

            List<Mesh> meshes = worldMeshStrat.GenerateAsChunks(world.GetChunks());
            exportStrat.ExportAsChunks(path, meshes, message);

            CloseCompletely();
        }

        public override void Enter()
        {
            base.Enter();
            gameObject.SetActive(true);

            saveAsButton.onClick.AddListener(SaveAs);
            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
            exportButton.onClick.AddListener(Export);
            exportAsChunksButton.onClick.AddListener(ExportAsChunks);
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
            exportAsChunksButton.onClick.RemoveListener(ExportAsChunks);
            exportButton.onClick.RemoveListener(Export);
        }

        private void OnApplicationQuit()
        {
            if (unsavedChanges)
                Save();
        }
    }
}