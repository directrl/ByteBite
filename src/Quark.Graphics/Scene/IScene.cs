using Quark.Graphics.Windowing;
using Silk.NET.OpenGL;

namespace Quark.Graphics.Scene {
	
	public interface IScene {

		public void Update(double delta);
		public void Render<T>(WindowBase<T> window, GL gl);
	}
}