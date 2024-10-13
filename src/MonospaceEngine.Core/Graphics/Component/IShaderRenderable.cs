using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.Component {
	
	public interface IShaderRenderable {
		
		public void Render(ShaderProgram program);
	}
}