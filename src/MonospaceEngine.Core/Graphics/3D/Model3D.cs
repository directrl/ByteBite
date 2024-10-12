using System.Numerics;

namespace MonospaceEngine.Graphics._3D {
	
	public class Model3D : Model {

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

		public Model3D(string id, params Mesh[] meshes) : base(id, meshes) { }
	}
}