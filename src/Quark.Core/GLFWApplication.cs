using System.Diagnostics;
using Quark.Graphics;
using Quark.Graphics.Windowing;
using Silk.NET.GLFW;

namespace Quark.Core {
	
	public unsafe class GLFWApplication : ApplicationBase {

		public List<GLFWWindow> Windows { get; } = new();
		
		public GLFWApplication(string id) : base(id) { }

		public override void Start(string[] args) {
			if(!GLFW.API.Init()) throw new GlfwException($"Could not initialize GLFW: {GLFW.GetErrorString()}");
			
			Initialize();
			Running = true;
			
			Debug.Assert(Windows.Count > 0);

			var primaryWindow = Windows[0];
			while(!GLFW.API.WindowShouldClose(primaryWindow.Handle) && Running) {
				foreach(var window in Windows) {
					window.Begin();
					window.Update();
					window.Render();
					window.End();
				}
			}

			Running = false;
		}
	}
}
