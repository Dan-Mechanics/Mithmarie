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
                new ExtensionFilter("Text", "txt")
            };

            string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "note", extensionList);
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path))
                File.WriteAllText(path, field.text);
        }

        private void Load()
        {
            // Open file
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
            if (paths.Length <= 0)
                return;

            string path = paths[0];
            field.text = File.ReadAllText(path);
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
