using System.Collections;
using Newtonsoft.Json.Linq;
using QuarkEngine.Configuration;
using QuarkEngine.Graphics.Scene;
using Silk.NET.Input;

namespace QuarkEngine.Input;

public class KeyBindings : IDisposable {

	public GameSettings Settings { get; }
	public Dictionary<string, KeyBinding> Bindings { get; } = new();

	private readonly List<Key> combo = new();

	public KeyBindings(SceneBase scene) {
		Settings = new(new FileInfo(Path.Combine(
			Directories.ConfigRoot.FullName, $"keys.{scene.Id}.json")));

		// Settings.SettingChange += (key, from, to) => {
		// 	if(Bindings.ContainsKey(key)) {
		// 		var jObject = (JObject) from;
		// 		var binding = jObject.ToObject<KeyBinding>();
		//
		// 		if(binding != null) {
		// 			Bindings[key] = binding;
		// 		} else {
		// 			Console.WriteLine($"Could not update changed keybinding {key}");
		// 		}
		// 	}
		// };
	}

	public void Register(ref KeyBinding binding) {
		if(Settings.Has(binding.Name)) {
			var temp = Settings.GetObject<KeyBinding>(binding.Name);
			binding.Keys = temp.Keys;
		} else {
			Settings.Set(binding.Name, binding);
		}

		Bindings[binding.Name] = binding;
	}
	
	public KeyBinding Register(KeyBinding tempBinding) {
		var binding = tempBinding;
		
		if(Settings.Has(binding.Name)) {
			binding = Settings.GetObject<KeyBinding>(binding.Name);
		} else {
			Settings.SetObject(binding.Name, binding);
		}

		Bindings[binding.Name] = binding;
		return binding;
	}

	public void Rebind(KeyBinding binding) {
		Bindings[binding.Name] = binding;
		Settings.SetObject(binding.Name, binding);
	}

	public void Input(IKeyboard keyboard, KeyAction action, Key key) {
		if(Bindings.Count <= 0) return;

		switch(action) {
			case KeyAction.Press:
				combo.Add(key);
				break;
			case KeyAction.Release:
				combo.Remove(key);
				break;
		}

		if(action == KeyAction.Press) {
			foreach(var binding in Bindings.Values) {
				if(binding.Keys.Length == 1) {
					binding.Pressed = (key == binding.Keys[0]);
				} else {
					binding.Pressed = binding.Keys.SequenceEqual(combo);
				}
			}
		}
	}

	public void Update(IKeyboard keyboard) {
		foreach(var binding in Bindings.Values) {
			binding.Pressed = false;

			if(binding.Keys.Length == 1) {
				binding.Down = keyboard.IsKeyPressed(binding.Keys[0]);
			}
		}
	}

	public void Dispose() {
		GC.SuppressFinalize(this);
		Settings.Save();
	}
}