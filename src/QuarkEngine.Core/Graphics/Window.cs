using System.Diagnostics;
using QuarkEngine.Configuration;
using QuarkEngine.Graphics.Scene;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace QuarkEngine.Graphics {

	public class Window {

		public IWindow Impl { get; }
		public IScene? Scene { get; set; }
		
		protected GL? GL { get; private set; }
		
		public Window(IWindow impl) {
			Impl = impl;
			Impl.UpdatesPerSecond = EngineSettings.UpdatesPerSecond;
			Impl.VSync = true;

			Impl.Load += () => {
				GL = Impl.CreateOpenGL();
			};

			Impl.FramebufferResize += size => {
				GL?.Viewport(size);
			};
			
			Impl.Update += delta => {
				Scene?.Update(delta);
			};
			
			Impl.Render += delta => {
				if(GL != null) Scene?.Render(GL, delta);
			};
			
			Impl.Initialize();
			Impl.IsVisible = true;
		}

		~Window() {
			GL?.Dispose();
			Impl.Reset();
			Impl.Dispose();
		}

		public void Close() {
			Impl.IsClosing = true;
		}

		public static Window Create(WindowOptions? options = null) {
			if(options == null) {
				return new(Silk.NET.Windowing.Window.Create(WindowOptions.Default));
			}

			return new(Silk.NET.Windowing.Window.Create(options.Value));
		}
	}
}