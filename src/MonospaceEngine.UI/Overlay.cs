using MonospaceEngine.Utilities;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace MonospaceEngine.UI {
	
	public delegate void OverlayRender(double delta);
	
	public class Overlay {

		protected readonly IWindow Window;
		protected readonly GL GL;

		public event OverlayRender? Render;
		
		public Overlay(IWindow window) {
			Window = window;
			GL = GLManager.Current;
		}

		public virtual void OnRender(double delta) {
			Render?.Invoke(delta);
		}
	}
}