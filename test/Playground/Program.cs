using Playground.Scenes;
using QuarkEngine.Core;
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
		}

		public static void Main(string[] args) {
			var app = new Program();
			app.Start(args);
		}
	}
}