using Silk.NET.OpenGL;

namespace QuarkEngine.Graphics.Scene {
	
	public interface IScene {

		public void OnLoad(Window window) {
			if(!window.Impl.IsVisible) window.Impl.IsVisible = true;
		}
		
		public void OnUnload() { }

		public void Update(double delta);
		public void Render(GL gl, double delta);
	}
}