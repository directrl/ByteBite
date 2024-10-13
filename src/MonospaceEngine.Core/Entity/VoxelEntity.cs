using Arch.Core;
using MonospaceEngine.Entity.Component;
using MonospaceEngine.Graphics;
using MonospaceEngine.Graphics._3D;
using MonospaceEngine.Graphics.Scene;
using Silk.NET.OpenGL;
using Object3D = MonospaceEngine.Graphics._3D.Object3D;
using Texture = MonospaceEngine.Graphics.Texture;

namespace MonospaceEngine.Entity {
	
	public struct VoxelEntity {

		/*public static readonly Mesh MESH = new(PrimitiveType.TriangleStrip,
			new float[] {
				-1, 1, 1,
				1, 1, 1,
				-1, -1, 1,
				1, -1, 1,
				-1, -1, -1,
				1, -1, -1,
				-1, 1, -1,
				1, 1, -1,
				-1, 1, -1,
				-1, -1, -1,
				1, -1, -1,
				1, -1, 1
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
		);*/

		public static readonly Mesh MESH = new(PrimitiveType.Triangles,
			new float[] {
				-0.5f, -0.5f, -0.5f,
				0.5f, -0.5f, -0.5f,
				0.5f,  0.5f, -0.5f,
				0.5f,  0.5f, -0.5f,
				-0.5f,  0.5f, -0.5f,
				-0.5f, -0.5f, -0.5f,

				-0.5f, -0.5f,  0.5f,
				0.5f, -0.5f,  0.5f,
				0.5f,  0.5f,  0.5f,
				0.5f,  0.5f,  0.5f,
				-0.5f,  0.5f,  0.5f,
				-0.5f, -0.5f,  0.5f,

				-0.5f,  0.5f,  0.5f,
				-0.5f,  0.5f, -0.5f,
				-0.5f, -0.5f, -0.5f,
				-0.5f, -0.5f, -0.5f,
				-0.5f, -0.5f,  0.5f,
				-0.5f,  0.5f,  0.5f,

				0.5f,  0.5f,  0.5f,
				0.5f,  0.5f, -0.5f,
				0.5f, -0.5f, -0.5f,
				0.5f, -0.5f, -0.5f,
				0.5f, -0.5f,  0.5f,
				0.5f,  0.5f,  0.5f,

				-0.5f, -0.5f, -0.5f,
				0.5f, -0.5f, -0.5f,
				0.5f, -0.5f,  0.5f,
				0.5f, -0.5f,  0.5f,
				-0.5f, -0.5f,  0.5f,
				-0.5f, -0.5f, -0.5f,

				-0.5f,  0.5f, -0.5f,
				0.5f,  0.5f, -0.5f,
				0.5f,  0.5f,  0.5f,
				0.5f,  0.5f,  0.5f,
				-0.5f,  0.5f,  0.5f,
				-0.5f,  0.5f, -0.5f,
			},
			new float[] {
				0, 0,
				1, 0,
				1, 1,
				1, 1,
				0, 1,
				0, 0,
				
				0, 0,
				1, 0,
				1, 1,
				1, 1,
				0, 1,
				0, 0,
				
				0, 0,
				1, 0,
				1, 1,
				1, 1,
				0, 1,
				0, 0,
				
				0, 0,
				1, 0,
				1, 1,
				1, 1,
				0, 1,
				0, 0,
				
				0, 0,
				1, 0,
				1, 1,
				1, 1,
				0, 1,
				0, 0,
				
				0, 0,
				1, 0,
				1, 1,
				1, 1,
				0, 1,
				0, 0,
			},
			new int[] {
				0, 1, 2, 3, 4, 5,
				6, 7, 8, 9, 10, 11,
				12, 13, 14, 15, 16, 17,
				18, 19, 20, 21, 22, 23,
				24, 25, 26, 27, 28, 29,
				30, 31, 32, 33, 34, 35
			}/*,
			new float[] {
				0.0f,  0.0f, -1.0f,
				0.0f,  0.0f, -1.0f,
				0.0f,  0.0f, -1.0f,
				0.0f,  0.0f, -1.0f,
				0.0f,  0.0f, -1.0f,
				0.0f,  0.0f, -1.0f,

				0.0f,  0.0f,  1.0f,
				0.0f,  0.0f,  1.0f,
				0.0f,  0.0f,  1.0f,
				0.0f,  0.0f,  1.0f,
				0.0f,  0.0f,  1.0f,
				0.0f,  0.0f,  1.0f,

				-1.0f,  0.0f,  0.0f,
				-1.0f,  0.0f,  0.0f,
				-1.0f,  0.0f,  0.0f,
				-1.0f,  0.0f,  0.0f,
				-1.0f,  0.0f,  0.0f,
				-1.0f,  0.0f,  0.0f,

				1.0f,  0.0f,  0.0f,
				1.0f,  0.0f,  0.0f,
				1.0f,  0.0f,  0.0f,
				1.0f,  0.0f,  0.0f,
				1.0f,  0.0f,  0.0f,
				1.0f,  0.0f,  0.0f,

				0.0f, -1.0f,  0.0f,
				0.0f, -1.0f,  0.0f,
				0.0f, -1.0f,  0.0f,
				0.0f, -1.0f,  0.0f,
				0.0f, -1.0f,  0.0f,
				0.0f, -1.0f,  0.0f,

				0.0f,  1.0f,  0.0f,
				0.0f,  1.0f,  0.0f,
				0.0f,  1.0f,  0.0f,
				0.0f,  1.0f,  0.0f,
				0.0f,  1.0f,  0.0f,
				0.0f,  1.0f,  0.0f
			}*/
		);

		public static Arch.Core.Entity Create(World world) {
			return world.Create(
				new WorldObject3D { Object = new(MESH, Material.DEFAULT_MATERIAL) }
			);
		}
	}
}