using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics._3D.Light {
	
	public class StaticLight : Light {
		
		private static readonly Material MATERIAL = new Material {
			mType = Material.Type.Light,
			mSpecularType = Material.SpecularType.Phong,
			AmbientColor = new(0.4f, 0.4f, 0.4f),
			DiffuseColor = new(1, 1, 1),
			SpecularColor = new(1, 1, 1),
			Metallic = 1
		};

		public StaticLight()
			: base(ModelLoader.Load(Monospace.EngineResources[ResourceType.MODEL, "voxel.gltf"]), MATERIAL) { }
	}
}