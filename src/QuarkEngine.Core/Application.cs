using System.Diagnostics;
using QuarkEngine.Graphics;
using QuarkEngine.Graphics.Scene;
using Silk.NET.GLFW;
using Silk.NET.Windowing;
using Window = QuarkEngine.Graphics.Window;

namespace QuarkEngine.Core {
	
	public class Application {
		
		public string Id { get; init; }
		public bool Running { get; protected set; }

		public List<Window> Windows { get; } = new();

		protected Application(string id) {
			Quark._currentApplication = this;
			Id = id;
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

			Running = false;
		}
	}
}