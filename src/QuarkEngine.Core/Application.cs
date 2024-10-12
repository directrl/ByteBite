using System.Diagnostics;
using QuarkEngine.Configuration;
using QuarkEngine.Graphics;
using QuarkEngine.Graphics.Scene;
using Silk.NET.GLFW;
using Silk.NET.Windowing;
using Window = QuarkEngine.Graphics.Window;

namespace QuarkEngine {
	
	public class Application {
		
		public string Id { get; init; }
		public bool Running { get; protected set; }

		public List<Window> Windows { get; } = new();
		public GameSettings GameSettings { get; }

		protected Application(string id) {
			Quark._currentApplication = this;
			Id = id;

			Directories.Resolve(id);

			GameSettings = new(new FileInfo(Path.Combine(
				Directories.ConfigRoot.FullName, "default.json")));

			AppDomain.CurrentDomain.ProcessExit += (s, e) => {
				Shutdown();
			};
		}
		
		public virtual void Initialize() { }
		public virtual void Update() { }

		public void Start(string[] args) {
			Initialize();
			Running = true;
			
			Debug.Assert(Windows.Count > 0);

			var primaryWindow = Windows.First();
			while(!primaryWindow.Impl.IsClosing && Running) {
				foreach(var window in Windows) {
					window.Impl.DoEvents();
					
					if(!window.Impl.IsClosing) window.Impl.DoUpdate();
					if(window.Impl.IsClosing) continue;
					window.Impl.DoRender();
				}
			}
		}

		public void Shutdown() {
			Console.WriteLine("Shutting down");
			Running = false;

			foreach(var window in Windows) {
				window.Dispose();
			}
			
			GameSettings.Save();
		}
	}
}