using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.OpenGL {
	
	public static class OpenGL {

		public static void EnableDefaults(GL gl) {
			gl.Enable(EnableCap.DepthTest);
			gl.Enable(EnableCap.CullFace);
			
			gl.CullFace(TriangleFace.Back);
		}
	}
}