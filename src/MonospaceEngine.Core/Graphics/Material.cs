using System.Drawing;
using System.Numerics;
using MonospaceEngine.Graphics.OpenGL;

namespace MonospaceEngine.Graphics {
	
	public struct Material {

		public static readonly Material DEFAULT_MATERIAL = new() {
			mType = Type.Regular,
			mSpecularType = SpecularType.Phong,
			Albedo = Color.FromArgb(255, 255, 255),
			AmbientColor = new(1, 1, 1),
			DiffuseColor = new(1, 1, 1),
			SpecularColor = new(1, 1, 1),
			Metallic = 64,
			Texture = Texture.DEFAULT_TEXTURE
		};
		
		public enum Type {
		
			Light,
			Regular
		}

		public enum SpecularType {
			
			Phong,
			Disabled
		}

		public Type mType;
		public SpecularType mSpecularType;

		public Color Albedo;
		
		public Vector3 AmbientColor;
		public Vector3 DiffuseColor;
		public Vector3 SpecularColor;

		public float Metallic;

		public Texture Texture;

		public void Load(ShaderProgram program, string target) {
			program.SetUniform($"{target}.type", (int) mType);
			program.SetUniform($"{target}.specularType", (int) mSpecularType);
			program.SetUniform($"{target}.albedo", new Vector3(Albedo.R / 255f,
																	Albedo.G / 255f,
																	Albedo.B / 255f));
			program.SetUniform($"{target}.ambient", AmbientColor);
			program.SetUniform($"{target}.diffuse", DiffuseColor);
			program.SetUniform($"{target}.specular", SpecularColor);
			program.SetUniform($"{target}.metallic", Metallic);
		}
	}
}