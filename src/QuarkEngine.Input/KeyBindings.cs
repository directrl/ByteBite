using Newtonsoft.Json.Linq;
using QuarkEngine.Configuration;

namespace QuarkEngine.Input;

public class KeyBindings {

	private readonly GameSettings settings;
	private readonly Dictionary<string, KeyBinding> keyBindings = new();

	public KeyBindings() {
		settings = new(new FileInfo(Path.Combine(
			Directories.ConfigRoot.FullName, "keys.json")));

		settings.SettingChange += (key, from, to) => {
			if(keyBindings.ContainsKey(key)) {
				var jObject = (JObject) from;
				var binding = jObject.ToObject<KeyBinding>();

				if(binding != null) {
					keyBindings[key] = binding;
				} else {
					Console.WriteLine($"Could not update changed keybinding {key}");
				}
			}
		};
	}

	public KeyBinding Register(KeyBinding binding) {
		var temp = binding;
		
		if(settings.Has(binding.Name)) {
			temp = settings.GetObject<KeyBinding>(binding.Name);
		} else {
			settings.Set(binding.Name, binding);
		}

		keyBindings[temp.Name] = temp;
		return temp;
	}
	
	public void Callback()
}