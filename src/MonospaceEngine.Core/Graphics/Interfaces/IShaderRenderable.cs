using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.Interfaces {
	
	public interface IShaderRenderable {
		
		public void Render(GL gl, ShaderProgram program);
	}
}