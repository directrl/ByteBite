using MonospaceEngine.Graphics.OpenGL;

namespace MonospaceEngine.Graphics._3D.Light {
	
	public abstract class Light : Object3D {
		
		protected Light(Mesh mesh, Material? material = null) : base(mesh, material) { }

		public override void Render(ShaderProgram shader) {
			shader.SetUniform("light.position", Position);
			Material.Load(shader, "light.material");
			base.Render(shader);
		}
	}
}