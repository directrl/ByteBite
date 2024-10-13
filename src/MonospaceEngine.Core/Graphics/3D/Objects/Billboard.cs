using System.Drawing;
using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Utilities;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics._3D.Objects {
	
	public class Billboard : Object3D {
		
		public Billboard(Texture texture) : base(new(
			GLManager.Current, "Builtin/Billboard",
			new List<Mesh>() {
				new Mesh(PrimitiveType.Triangles,
					new float[] {
						0, 0, 0,
						0, 0, 1,
						1, 0, 0,
						
						1, 0, 1,
						0, 0, 1,
						1, 0, 0
					},
					new float[] { },
					new float[] {
						0, 0,
						0, 1,
						1, 0,
						
						1, 1,
						0, 1,
						1, 0
					},
					new uint[] {
						1, 2, 3,
						4, 5, 6
					})
			},
			new List<Material>() {
				new Material {
					mType = Material.Type.Regular,
					mSpecularType = Material.SpecularType.Phong,
					Albedo = Color.FromArgb(255, 255, 255),
					AmbientColor = new(1, 1, 1),
					DiffuseColor = new(1, 1, 1),
					SpecularColor = new(1, 1, 1),
					Metallic = 32,
					DiffuseMaps = new() { texture }
				}
			}
		)) { }

		public override void Render(ShaderProgram shader) {
			_gl.Disable(EnableCap.CullFace);
			base.Render(shader);
			_gl.Enable(EnableCap.CullFace);
		}
	}
}