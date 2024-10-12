using MonospaceEngine.Logging;
using Newtonsoft.Json.Linq;
using Playground.Scenes;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Window = MonospaceEngine.Graphics.Window;

namespace Playground {

	public class Program : MonospaceEngine.Monospace {

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