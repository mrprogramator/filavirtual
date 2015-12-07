using Newtonsoft.Json;
using SistemaDeGestionDeFilas.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SistemaDeGestionDeFilas.Services
{
    public class ClientAppSettings
    {
        [JsonProperty("URL")]
        public String ServerURL { get; set; }

        [JsonIgnore]
        public String Filename { get; set; }

        [JsonProperty("width")]
        public Int32 ScreenWidth { get; set; }

        [JsonProperty("height")]
        public Int32 ScreenHeight { get; set; }

        [JsonProperty("openDevTools")]
        public Boolean OpenDevTools { get; set; }
    }

    public class ClientAppPackager
    {
        #region Singleton

        private static Lazy<ClientAppPackager> _instance = new Lazy<ClientAppPackager>(() => new ClientAppPackager());

        public static ClientAppPackager Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        private String _outputDir;

        private ClientAppPackager()
        {
        }

        public void Initialize()
        {
            _outputDir = DirectoryHelpers.CreateTemporary();
        }

        public String CreatePackage(ClientAppSettings settings, Boolean useCache = true)
        {
            var zipPath = GeneratePackagePath(settings);

            if (useCache && System.IO.File.Exists(zipPath))
            {
                return zipPath;
            }

            GeneratePackage(zipPath, settings);

            return zipPath;
        }

        private String GeneratePackagePath(ClientAppSettings settings)
        {
            if (_outputDir == null)
            {
                _outputDir = DirectoryHelpers.CreateTemporary();
            }
            return Path.Combine(_outputDir, settings.Filename + ".zip");
        }

        private void GeneratePackage(String zipPath, ClientAppSettings settings)
        {
            var server = HttpContext.Current.Server;

            var buildDir = DirectoryHelpers.CreateTemporary();
            var buildAppPath = Path.Combine(buildDir, "resources", "app");
            var settingsPath = Path.Combine(buildAppPath, "config.json");

            var appPath = server.MapPath("~/Resources/operador/app");
            var releasePath = server.MapPath("~/Resources/operador/releases/" + settings.Filename);

            DirectoryHelpers.Copy(releasePath, buildDir, copySubDirs: true);
            DirectoryHelpers.Copy(appPath, buildAppPath, copySubDirs: true);

            var settingsJson = JsonConvert.SerializeObject(settings);
            File.WriteAllText(settingsPath, settingsJson);

            System.IO.Compression.ZipFile.CreateFromDirectory(buildDir, zipPath, System.IO.Compression.CompressionLevel.NoCompression, includeBaseDirectory: false);
        }
    }
}