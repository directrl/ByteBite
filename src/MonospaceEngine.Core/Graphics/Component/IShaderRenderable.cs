using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.Components {
	
	public interface IShaderRenderable {
		
		public void Render(GL gl, ShaderProgram program);
	}
}