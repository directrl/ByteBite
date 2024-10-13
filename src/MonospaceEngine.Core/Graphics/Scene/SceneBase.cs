using MonospaceEngine.Graphics.Component;
using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Input;
using MonospaceEngine.Utilities;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.Scene {
	
	public abstract class SceneBase : IDisposable {

		public GL GL { get; protected set; }
		public Window? Window { get; private set; }
		
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
			Window = window;
		}

		public virtual void OnUnload() {
			Window = null;
		}

		public abstract void Update(float delta);
		public virtual void Render(float delta) {
			GL = GLManager.Current;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			KeyBindings.Dispose();
		}
	}
}