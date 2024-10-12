using Newtonsoft.Json.Linq;
using Playground.Scenes;
using Monospace.Configuration;
using Monospace.Logging;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Window = Monospace.Graphics.Window;

namespace Playground {

	public class Program : Monospace.Monospace {

		private Program() : base("playground") {
			AppLogger = LoggerFactory.CreateDefaultConfiugration(LoggerPurpose.Application).CreateLogger();
			AppResources = new("Playground");
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