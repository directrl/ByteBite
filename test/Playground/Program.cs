using Newtonsoft.Json.Linq;
using Playground.Scenes;
using Monospace;
using Monospace.Configuration;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Window = Monospace.Graphics.Window;

namespace Playground {

	public class Program : Application {

		private Program() : base("playground") {
			GameResources = new("Playground");
		}

		public override void Initialize() {
			var window = Window.Create();
			var scene = new Test3DScene();
			window.Scene = scene;

			Windows.Add(window);
		}

		public static void Main(string[] args) {
			var app = new Program();
			app.Start(args);
		}
	}
}