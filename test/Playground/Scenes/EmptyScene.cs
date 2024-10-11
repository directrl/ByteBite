using QuarkEngine.Graphics;
using QuarkEngine.Graphics.Scene;
using Silk.NET.OpenGL;

namespace Playground.Scenes {
	
	public class EmptyScene : IScene {

		public void Update(double delta) { }
		public void Render(GL gl, double delta) { }
	}
}