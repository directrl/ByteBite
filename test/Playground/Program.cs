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
			AppResources = new("Playground.Assets");
		}

		public override void Initialize() {
			var windowOptions = WindowOptions.Default;
			var gAPI = GraphicsAPI.Default;
			gAPI.Flags = ContextFlags.Debug | ContextFlags.ForwardCompatible;
			windowOptions.API = gAPI;
			var window = Window.Create(windowOptions);
			var scene = new WorldTestScene();
			window.Scene = scene;

			Windows.Add(window);
		}

		public static void Main(string[] args) {
			var app = new Program();
			app.Start(args);
		}
	}
}