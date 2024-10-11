using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuarkEngine.Configuration {

	public delegate void SettingChangeEvent(string key, object from, object to);
	
	public class GameSettings : IDisposable {
		
		private FileInfo ConfigFile { get; }
		private JObject JsonConfig { get; }

		public event SettingChangeEvent? SettingChange;

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
		
		public T Get<T>(string key) {
			var value = JsonConfig[key];
			if(value != null) return value.Value<T>();
			return default;
		}
		
		public T GetObject<T>(string key) {
			var value = JsonConfig[key];
			if(value != null) return value.ToObject<T>();
			return default;
		}

		public void Set<T>(string key, T value) {
			var prevValue = JsonConfig[key];
			if(prevValue != null) OnSettingChange(key, prevValue.Value<T>(), value);
			JsonConfig[key] = JToken.FromObject(value);
		}
		
		public void Dispose() {
			using(var writer = ConfigFile.CreateText())
			using(var json = new JsonTextWriter(writer)) {
				json.Indentation = 4;
				json.Formatting = Formatting.Indented;
				
				JsonConfig.WriteTo(json);
			}
		}

		protected void OnSettingChange(string key, object from, object to) {
			SettingChange?.Invoke(key, from, to);
		}
	}
}