using Quark.Graphics.Windowing;
using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;

namespace Quark.Graphics {
	
	public unsafe class OpenGL {

		public const int TARGET_VERSION_MAJOR = 3;
		public const int TARGET_VERSION_MINOR = 3;

		public static IGLContext CreateContext<T>(WindowBase<T> window, IGLContextSource? source = null) {
			switch(window) {
				case GLFWWindow glfwWindow:
					return new GlfwContext(GLFW.API, glfwWindow.Handle, source);
				default:
					throw new NotImplementedException("Not implemented for request window type");
			}
		}
	}
}