using Monospace.Input;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace Monospace.Graphics.Scene {
	
	public abstract class SceneBase : IDisposable {
		
		public string Id { get; }
		public KeyBindings KeyBindings { get; }
		public IMouse? Mouse { get; set; }

		protected SceneBase(string id) {
			Id = id;
			KeyBindings = new(this);
		}

		public virtual void OnLoad(Window window) {
			if(!window.Impl.IsVisible) window.Impl.IsVisible = true;
		}
		
		public virtual void OnUnload() { }

		public abstract void Update(double delta);
		public abstract void Render(GL gl, double delta);

		public void Dispose() {
			GC.SuppressFinalize(this);
			KeyBindings.Dispose();
		}
	}
}