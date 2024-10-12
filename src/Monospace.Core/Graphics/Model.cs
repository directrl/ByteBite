using Monospace.Graphics.Interfaces;
using Silk.NET.OpenGL;

namespace Monospace.Graphics {
	
	public class Model : IRenderable {
		
		public string Id { get; }
		public Mesh[] Meshes { get; }

		public Model(string id, params Mesh[] meshes) {
			Id = id;
			Meshes = meshes;
		}

		public void Render(GL gl) {
			foreach(var mesh in Meshes) {
				mesh.Render();
			}
		}
	}
}