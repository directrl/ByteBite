using System.Diagnostics;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Monitor = Silk.NET.GLFW.Monitor;
using Rectangle = System.Drawing.Rectangle;

namespace Quark.Graphics.Windowing {
	
	public unsafe class GLFWWindow : WindowBase<WindowHandle> {

		private static WindowHandle* sharedHandle;

		private string _title = "GLFW Window";
		public override string Title {
			get => _title;
			set {
				_title = value;
				GLFW.API.SetWindowTitle(Handle, _title);
			}
		}

		public override Vector2D<int> Dimensions {
			get {
				int width, height;
				GLFW.API.GetWindowSize(Handle, out width, out height);
				return new(width, height);
			}
			set => GLFW.API.SetWindowSize(Handle, value.X, value.Y);
		}

		public override Vector2D<int> Position {
			get {
				int x, y;
				GLFW.API.GetWindowPos(Handle, out x, out y);
				return new(x, y);
			}
			set {
				if(value.X <= 0 || value.Y <= 0) {
					VideoMode* videoMode;

					if(Monitor != null) videoMode = GLFW.API.GetVideoMode(Monitor);
					else videoMode = GLFW.API.GetVideoMode(GLFW.API.GetPrimaryMonitor());

					var dim = Dimensions;
					
					GLFW.API.SetWindowPos(
						Handle,
						(videoMode->Width / 2) - (dim.X / 2),
						(videoMode->Height / 2) - (dim.Y / 2)
					);
				} else {
					GLFW.API.SetWindowPos(Handle, value.X, value.Y);
				}
			}
		}

		public Monitor* Monitor {
			get => GLFW.API.GetWindowMonitor(Handle);
			set => GLFW.API.SetWindowMonitor(
				Handle,
				value,
				Position.X, Position.Y,
				Dimensions.X, Dimensions.Y,
				-1
			);
		}

		public GLFWWindow() {
			GLFW.API.WindowHint(WindowHintInt.ContextVersionMajor, OpenGL.TARGET_VERSION_MAJOR);
			GLFW.API.WindowHint(WindowHintInt.ContextVersionMinor, OpenGL.TARGET_VERSION_MINOR);
			GLFW.API.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
			GLFW.API.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
			
			GLFW.API.WindowHint(WindowHintBool.Visible, false);

			Handle = GLFW.API.CreateWindow(
				512, 512,
				Title,
				null,
				sharedHandle
			);

			if(Handle == null) throw new GlfwException($"Could not create a GLFW window: {GLFW.GetErrorString()}");
			if(sharedHandle == null) sharedHandle = Handle;
			
			GLContext = OpenGL.CreateContext(this);
			GL = GL.GetApi(GLContext);
		}

		~GLFWWindow() {
			GLFW.API.DestroyWindow(Handle);
		}

		public override void Show() {
			GLFW.API.ShowWindow(Handle);
		}

		public override void Hide() {
			GLFW.API.HideWindow(Handle);
		}

		public override void Begin() {
			base.Begin();
			
			GLFW.API.MakeContextCurrent(Handle);
			GLContext.MakeCurrent();

			int fbWidth, fbHeight;
			GLFW.API.GetFramebufferSize(Handle, out fbWidth, out fbHeight);
			
			GL.Viewport(new Rectangle(0, 0, fbWidth, fbHeight));
			GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public override void End() {
			Debug.Assert(GLContext.IsCurrent,
				"OpenGL context is not current (called End() without Begin()?)");
			
			GLFW.API.SwapBuffers(Handle);
			GLFW.API.PollEvents();
			
			base.End();
		}
	}
}