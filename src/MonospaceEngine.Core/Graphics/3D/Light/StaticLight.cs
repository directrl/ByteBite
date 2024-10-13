using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics._3D.Light {
	
	public class StaticLight : Light {
		
		private static readonly Mesh MESH = new(PrimitiveType.TriangleStrip,
			new float[] {
				-0.5f, 0.5f, 0.5f,
				0.5f, 0.5f, 0.5f,
				-0.5f, -0.5f, 0.5f,
				0.5f, -0.5f, 0.5f,
				-0.5f, -0.5f, -0.5f,
				0.5f, -0.5f, -0.5f,
				-0.5f, 0.5f, -0.5f,
				0.5f, 0.5f, -0.5f,
				-0.5f, 0.5f, -0.5f,
				-0.5f, -0.5f, -0.5f,
				0.5f, -0.5f, -0.5f,
				0.5f, -0.5f, 0.5f
			},
			new float[] {
				0, 0,
				1, 0,
				0, 1,
				1, 1,
				0, 0,
				1, 0,
				0, 1,
				1, 1,
				1, 0,
				1, 1,
				0, 1,
				0, 0
			},
			new int[] {
				8, 9, 0, 2, 1, 3,
				3, 2, 5, 4, 7, 6,
				6, 0, 7, 1, 10, 11
			}
		);

		private static readonly Material MATERIAL = new() {
			mType = Material.Type.Light,
			mSpecularType = Material.SpecularType.Phong,
			AmbientColor = new(0.4f, 0.4f, 0.4f),
			DiffuseColor = new(1, 1, 1),
			SpecularColor = new(1, 1, 1),
			Metallic = 1
		};

		public StaticLight() : base(MESH, MATERIAL) {
			Material.Texture = Texture.Create(Monospace.EngineResources[ResourceType.TEXTURE, "light"]);
		}
	}
}