using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SFB;
using System.Linq;
using System.IO;

namespace Mitholca
{
    public class Notes : MonoBehaviour, IBinarySerializable
    {
        [SerializeField] private TMP_InputField field = default;
        [SerializeField] private Button saveButton = default;
        [SerializeField] private Button loadButton = default;

        private void Start()
        {
            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
        }

        private void OnDestroy()
        {
            saveButton.onClick.RemoveListener(Save);
            loadButton.onClick.RemoveListener(Load);
        }

        private void Save() 
        {
            var extensionList = new[] {
              //  new ExtensionFilter("Binary", "bin"),
                new ExtensionFilter("Mitholca", "mth")
            };

            string path = StandaloneFileBrowser.SaveFilePanel("Save As", "", "level", extensionList);
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                return;

            FileStream stream = File.OpenWrite(path);
            BinaryWriter writer = new BinaryWriter(stream);
            Serialize(writer);

            // This automatically closes the stream
            writer.Flush();
            writer.Close();

            field.text = string.Empty;
        }

        private void Load()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "mth", false);
            if (paths.Length <= 0)
                return;

            string path = paths[0];
            FileStream stream = File.OpenRead(path);
            BinaryReader reader = new BinaryReader(stream);
            Deserialize(reader);

            reader.Close();
            stream.Close();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(field.text);
        }

        public void Deserialize(BinaryReader reader)
        {
            field.text = reader.ReadString();
        }
    }
}
