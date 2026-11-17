using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ImageStitcher
{
    /// <summary>
    /// Represents a keyboard shortcut binding
    /// </summary>
    public class KeyboardBinding
    {
        [JsonProperty("key")]
        public string Key { get; set; }  // E.g., "Q", "W", "E", "R", "Z", "Alt+Q", "Shift+W"

        [JsonProperty("destinationFolder")]
        public string DestinationFolder { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public KeyboardBinding() { }

        public KeyboardBinding(string key, string destinationFolder, string description = "")
        {
            Key = key;
            DestinationFolder = destinationFolder;
            Description = description;
        }
    }

    /// <summary>
    /// Manages keyboard shortcut bindings from configuration file
    /// </summary>
    public class KeyboardShortcutManager
    {
        private Dictionary<Keys, string> _shortcutBindings = new Dictionary<Keys, string>();
        private string _configPath;

        public KeyboardShortcutManager(string configFileName = "KeyboardShortcuts.json")
        {
            // Use AppData folder for configuration
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ImageStitcher");

            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);

            _configPath = Path.Combine(appDataPath, configFileName);
        }

        /// <summary>
        /// Load keyboard bindings from configuration file
        /// </summary>
        public void LoadBindings()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    CreateDefaultConfigFile();
                    return;
                }

                string jsonContent = File.ReadAllText(_configPath);
                List<KeyboardBinding> bindings = JsonConvert.DeserializeObject<List<KeyboardBinding>>(jsonContent);

                _shortcutBindings.Clear();

                foreach (var binding in bindings)
                {
                    Keys keyData = ParseKeyString(binding.Key);
                    if (keyData != Keys.None && !string.IsNullOrWhiteSpace(binding.DestinationFolder))
                    {
                        // Normalize path: convert forward slashes to backslashes for Windows
                        string normalizedPath = binding.DestinationFolder.Replace('/', '\\');
                        _shortcutBindings[keyData] = normalizedPath;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Loaded {_shortcutBindings.Count} keyboard shortcuts from {_configPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading keyboard shortcuts: {ex}");
            }
        }

        /// <summary>
        /// Parse a key string like "Q", "Alt+Q", "Shift+W", "Ctrl+Alt+E"
        /// </summary>
        private Keys ParseKeyString(string keyString)
        {
            if (string.IsNullOrWhiteSpace(keyString))
                return Keys.None;

            Keys result = Keys.None;
            string[] parts = keyString.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                string trimmed = part.Trim();

                // Try to parse as modifier
                if (trimmed.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                    result |= Keys.Alt;
                else if (trimmed.Equals("Ctrl", StringComparison.OrdinalIgnoreCase) || trimmed.Equals("Control", StringComparison.OrdinalIgnoreCase))
                    result |= Keys.Control;
                else if (trimmed.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                    result |= Keys.Shift;
                else
                {
                    // Try to parse as key name
                    if (Enum.TryParse<Keys>(trimmed, true, out Keys key))
                    {
                        result |= key;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check if a key combination has a binding
        /// </summary>
        public bool TryGetDestinationFolder(Keys keyData, out string destinationFolder)
        {
            return _shortcutBindings.TryGetValue(keyData, out destinationFolder);
        }

        /// <summary>
        /// Get all bindings
        /// </summary>
        public Dictionary<Keys, string> GetAllBindings()
        {
            return new Dictionary<Keys, string>(_shortcutBindings);
        }

        /// <summary>
        /// Create a default configuration file if it doesn't exist
        /// </summary>
        private void CreateDefaultConfigFile()
        {
            try
            {
                List<KeyboardBinding> defaultBindings = new List<KeyboardBinding>
                {
                    new KeyboardBinding("Q", "E:/_FF/test", "Move to E:\\_FF\\test folder"),
                    new KeyboardBinding("W", "", "Move to folder 2 - Set destinationFolder to enable"),
                    new KeyboardBinding("E", "", "Move to folder 3 - Set destinationFolder to enable"),
                    new KeyboardBinding("Z", "", "Move to folder 4 - Set destinationFolder to enable"),
                    new KeyboardBinding("Alt+Q", "", "Move with Alt+Q - Set destinationFolder to enable"),
                    new KeyboardBinding("Alt+W", "", "Move with Alt+W - Set destinationFolder to enable")
                };

                string json = JsonConvert.SerializeObject(defaultBindings, Formatting.Indented);
                File.WriteAllText(_configPath, json);

                System.Diagnostics.Debug.WriteLine($"Created default keyboard shortcuts config at {_configPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating default config: {ex}");
            }
        }

        /// <summary>
        /// Get the configuration file path
        /// </summary>
        public string GetConfigPath()
        {
            return _configPath;
        }
    }
}
