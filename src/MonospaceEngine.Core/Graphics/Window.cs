using MonospaceEngine.Configuration;
using MonospaceEngine.Graphics.Scene;
using MonospaceEngine.Input;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace MonospaceEngine.Graphics {

	public class Window : IDisposable {

		public IWindow Impl { get; }

		private SceneBase? _scene;
		public SceneBase? Scene {
			get => _scene;
			set {
				value?.OnUnload();
				_scene = value;

				if(Input != null) {
					if(Input.Mice.Count > 0 && Scene != null) {
						Scene.Mouse = Input.Mice[0];
					}
				}
				
				value?.OnLoad(this);
			}
		}

		public GL? GL { get; private set; }
		public IInputContext? Input { get; private set; }
		
		public Window(IWindow impl) {
			Impl = impl;
			Impl.UpdatesPerSecond = EngineSettings.UpdatesPerSecond;
			Impl.VSync = true;

			Impl.Load += () => {
				GL = Impl.CreateOpenGL();
				Input = Impl.CreateInput();
				
				GL.Viewport(Impl.FramebufferSize);
				
				foreach(var keyboard in Input.Keyboards) {
					keyboard.KeyUp += (keyboard, key, scancode) => {
						Scene?.KeyBindings.Input(keyboard, KeyAction.Release, key);
					};
				
					keyboard.KeyDown += (keyboard, key, scancode) => {
						Scene?.KeyBindings.Input(keyboard, KeyAction.Press, key);
					};
				}
			};

			Impl.FramebufferResize += size => {
				GL?.Viewport(size);
			};
			
			Impl.Update += delta => {
				Scene?.Update(delta);

				if(Input != null) {
					if(Scene != null) Scene.Keyboard = Input.Keyboards[0];
					
					foreach(var keyboard in Input.Keyboards) {
						Scene?.KeyBindings.Update(keyboard);
					}
				}
			};
			
			Impl.Render += delta => {
				if(GL != null) {
					OpenGL.OpenGL.EnableDefaults(GL);
					
					GL.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
					Scene?.Render(GL);
					
					var error = GL.GetError();

					switch(error) {
						case GLEnum.NoError:
							break;
						default:
							Monospace.EngineLogger.Error($"OpenGL Error: {error}");
							break;
					}
				}
			};
			
			Impl.Initialize();
			Impl.IsVisible = true;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			
			Scene?.Dispose();
			GL?.Dispose();
			Input?.Dispose();
			Impl.Reset();
			Impl.Dispose();
		}

		public void Close() {
			Impl.IsClosing = true;
			Scene?.OnUnload();
		}

		public static Window Create(WindowOptions? options = null) {
			if(options == null) {
				return new(Silk.NET.Windowing.Window.Create(WindowOptions.Default));
			}

			return new(Silk.NET.Windowing.Window.Create(options.Value));
		}
	}
}