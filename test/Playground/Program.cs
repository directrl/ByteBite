using Newtonsoft.Json.Linq;
using Playground.Scenes;
using QuarkEngine;
using QuarkEngine.Configuration;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Window = QuarkEngine.Graphics.Window;

namespace Playground {

	public class Program : Application {
		
		private Program() : base("playground") { }

		public override void Initialize() {
			var window = Window.Create();
			window.Scene = new EmptyScene();

			Windows.Add(window);

			string sT = GameSettings.GetOrDefault("st", "hello");
			double dT = GameSettings.GetOrDefault("dt", 4.642);
			float fT = GameSettings.GetOrDefault("ft", 598.3129f);
			JObject a = new JObject();
			a["test"] = 3;
			a["ads"] = "sdasd";
			JObject joT = GameSettings.GetOrDefault("jot", a);
			byte bT = GameSettings.GetOrDefault("bt", (byte) 32);
		}

		public static void Main(string[] args) {
			var app = new Program();
			app.Start(args);
		}
	}
}