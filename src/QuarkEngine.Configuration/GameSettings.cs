using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuarkEngine.Configuration {
	
	public class GameSettings : IDisposable {
		
		private FileInfo ConfigFile { get; }
		private JObject JsonConfig { get; }

		public GameSettings(FileInfo configFile) {
			if(!configFile.Exists) {
				using(var writer = configFile.CreateText()) {
					writer.WriteLine("{}");
				}
			}

			ConfigFile = configFile;
			JsonConfig = JObject.Parse(File.ReadAllText(configFile.FullName));
		}

		public T GetOrDefault<T>(string key, T defaultValue) {
			var value = JsonConfig[key];
			if(value != null) return value.Value<T>();

			JsonConfig[key] = JToken.FromObject(defaultValue);
			return defaultValue;
		}
		
		public void Dispose() {
			using(var writer = ConfigFile.CreateText())
			using(var json = new JsonTextWriter(writer)) {
				json.Indentation = 4;
				json.Formatting = Formatting.Indented;
				
				JsonConfig.WriteTo(json);
			}
		}
	}
}