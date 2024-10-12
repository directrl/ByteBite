using MonospaceEngine.Graphics.Interfaces;
using MonospaceEngine.Input;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.Scene {
	
	public abstract class SceneBase : IDisposable, IRenderable {
		
		public string Id { get; }
		public KeyBindings KeyBindings { get; }
		
		public IMouse? Mouse { get; set; }
		public IKeyboard? Keyboard { get; set; }

		protected SceneBase(string id) {
			Id = id;
			KeyBindings = new(this);
		}

		public virtual void OnLoad(Window window) {
			if(!window.Impl.IsVisible) window.Impl.IsVisible = true;
		}
		
		public virtual void OnUnload() { }

		public abstract void Update(double delta);
		public abstract void Render(GL gl);

		void IRenderable.Render(GL gl) {
			Render(gl);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			KeyBindings.Dispose();
		}
	}
}