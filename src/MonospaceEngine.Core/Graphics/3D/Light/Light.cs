using MonospaceEngine.Graphics.OpenGL;

namespace MonospaceEngine.Graphics._3D.Light {
	
	public abstract class Light : Object3D {

		private readonly Material _lightMaterial;
		
		protected Light(Model model, Material material) : base(model) {
			_lightMaterial = material;
		}

		public override void Render(ShaderProgram shader) {
			shader.SetUniform("light.position", Position);
			_lightMaterial.Load(shader, "light.material");
			base.Render(shader);
		}
	}
}