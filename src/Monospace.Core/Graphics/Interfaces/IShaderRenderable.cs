using Monospace.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace Monospace.Graphics.Interfaces {
	
	public interface IShaderRenderable {
		
		public void Render(GL gl, ShaderProgram program);
	}
}