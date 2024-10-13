using MonospaceEngine.Graphics.Component;
using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics {
	
	public class Model : IShaderRenderable {

		public readonly Mesh Mesh;
		public Material Material;

		public Model(Mesh mesh, Material? material = null) {
			Mesh = mesh;
			Material = material ?? Material.DEFAULT_MATERIAL;
		}

		public virtual void Render(ShaderProgram shader) {
			Material.Load(shader, "material");
			Material.Texture.Bind();
			Mesh.Render();
		}
	}
}