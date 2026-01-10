using SFB;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
        [SerializeField] private Button exportAsButton = default;
        [SerializeField] private PersistentBool exportAsChunks = default;

        private World world;
        private IBinarySerializable level;
        private IMessageService message;
        private IExportStrategy exportStrat;
        private IWorldMeshStrategy worldMeshStrat;
        private string savePath;
        private string exportPath;
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

        private bool CheckUnsavedChanges()
        {
            if (!unsavedChanges)
                return false;

            message.Send("You have unsaved changes!\nTry again to confirm.", Color.red, 3.0f);
            unsavedChanges = false;

            return true;
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

        public async void Export()
        {
            if (!Utils.IsStringValid(exportPath))
            {
                ExportAs();
                return;
            }

            message.Send("Exporting ...", Color.gray, MESSAGE_DURATION);
            world.Flush();
            CloseCompletely();

            if (exportAsChunks.value)
            {
                await ExportChunksAsync();
            }
            else
            {
                //await ExportWholeAsync();
                await Task.Run(ExportWholeAsync);
            }
   
        }

        private async Task ExportWholeAsync()
        {
            //Mesh mesh = worldMeshStrat.GenerateMesh(world.GetWorldBlocks());
            MeshData data = GreedyWorldMesh._GenerateMesh(world.GetWorldBlocks());
            // exportStrat.Export(exportPath, mesh, message);
            OBJ._Export(exportPath, data, message);

            Debug.LogWarning("ExportWholeAsync() done.");
            await Task.CompletedTask;
        }

        /// <summary>
        /// TODO: FIX.
        /// </summary>
        private async Task ExportChunksAsync()
        {
            List<Mesh> meshes = worldMeshStrat.GenerateAsChunks(world.GetChunks());
            exportStrat.ExportAsChunks(exportPath, meshes, message);

            Debug.LogWarning("ExportChunksAsync() done.");
            await Task.CompletedTask;
        }

        private void ExportAs()
        {
            ExtensionFilter[] extensionList = new[] { new ExtensionFilter(exportStrat.GetWholeName(), exportStrat.GetShortName()) };

            string path = StandaloneFileBrowser.SaveFilePanel("Export As", "", "level", extensionList);
            if (!Utils.IsStringValid(path))
                return;

            exportPath = path;
            Export();
        }

        public override void Enter()
        {
            base.Enter();

            saveButton.onClick.AddListener(Save);
            saveAsButton.onClick.AddListener(SaveAs);
            loadButton.onClick.AddListener(Load);
            exportButton.onClick.AddListener(Export);
            exportAsButton.onClick.AddListener(ExportAs);
            newButton.onClick.AddListener(New);
        }

        public override void Exit()
        {
            base.Exit();

            saveButton.onClick.RemoveListener(Save);
            saveAsButton.onClick.RemoveListener(SaveAs);
            loadButton.onClick.RemoveListener(Load);
            exportButton.onClick.RemoveListener(Export);
            exportAsButton.onClick.RemoveListener(ExportAs);
            newButton.onClick.RemoveListener(New);
        }

#if NOT_UNITY_EDITOR
        private void OnApplicationQuit()
        {
            if (unsavedChanges)
                Save();
        }
#endif
    }
}