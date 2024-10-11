using Quark.Graphics.Scene;
using Quark.Graphics.Windowing;
using Silk.NET.OpenGL;

namespace Playground.Scenes {
	
	public class EmptyScene : IScene {

		public void Update(double delta) { }
		public void Render<T>(WindowBase<T> window, GL gl) { }
	}
}