using System.Numerics;
using MonospaceEngine.Graphics.Component;
using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Utilities;

namespace MonospaceEngine.Graphics._3D {
	
	public class Object3D : Model {

		//public Model? Model;

		public Vector3 Position = new();
		public Vector3 Rotation = new();
		public Vector3 Scale = new(1.0f, 1.0f, 1.0f);

		public Matrix4x4 ModelMatrix {
			get {
				var positionMatrix = Matrix4x4.CreateTranslation(Position);
				var rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
				var scaleMatrix = Matrix4x4.CreateScale(Scale);

				return positionMatrix * rotationMatrix * scaleMatrix;
			}
		}
		
		public Object3D(Model model) : base(GLManager.Current, model) { }

		public override void Render(ShaderProgram shader) {
			shader.SetUniform("model", ModelMatrix);
			base.Render(shader);
		}
	}
}