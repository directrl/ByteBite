using MonospaceEngine.Graphics.Scene;
using MonospaceEngine.Input;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace Playground.Scenes {
	
	public class InputTestScene : SceneBase {

		private KeyBinding testBinding;
		private KeyBinding test2Binding;
		private KeyBinding test3Binding;
		private KeyBinding test4Binding;

		public InputTestScene() : base("input_test") {
			testBinding = KeyBindings.Register(new("test", Key.ControlLeft, Key.A));
			test2Binding = KeyBindings.Register(new("test2", Key.ControlLeft, Key.S));
			test3Binding = KeyBindings.Register(new("test3", Key.ShiftLeft));
			test4Binding = KeyBindings.Register(new("test4", Key.ControlLeft, Key.ShiftLeft, Key.R));
		}

		public override void Update(double delta) {
			if(testBinding.Pressed) {
				Console.WriteLine("hello!");
			}
			
			if(test2Binding.Pressed) {
				Console.WriteLine("meow!");
			}
			
			if(test3Binding.Pressed) {
				Console.WriteLine("woof!");
			}
			
			if(test3Binding.Down) {
				Console.WriteLine("woofsda2!");
			}
			
			if(test4Binding.Pressed) {
				Console.WriteLine("awoo!");

				test4Binding.Keys = new[] { Key.ControlLeft, Key.ShiftLeft, Key.G };
				KeyBindings.Rebind(test4Binding);
			}
			
			if(Mouse?.IsButtonPressed(MouseButton.Left) ?? false) {
				Console.WriteLine("aaaa");
			}
		}
		
		public override void Render() { }
	}
}