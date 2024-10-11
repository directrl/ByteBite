using Playground.Scenes;
using Quark.Core;
using Quark.Graphics.Windowing;
using Silk.NET.Maths;

namespace Playground {

	public class Program : GLFWApplication {
		
		private Program() : base("playground") { }

		public override void Initialize() {
			var window = new GLFWWindow {
				Position = new Vector2D<int>(-1, -1),
				Scene = new EmptyScene(),
				Title = "Playground"
			};

			window.Show();
			Windows.Add(window);
		}

		public static void Main(string[] args) {
			var app = new Program();
			app.Start(args);
		}
	}
}