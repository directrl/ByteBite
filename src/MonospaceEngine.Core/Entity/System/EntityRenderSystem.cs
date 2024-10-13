using Arch.Core;
using MonospaceEngine.Entity.Component;
using MonospaceEngine.Graphics;
using MonospaceEngine.Graphics._3D;
using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Entity.System {
	
	public class EntityRenderSystem : SystemBase<ShaderProgram, Camera3D> {

		private readonly QueryDescription _entityQuery = new QueryDescription().WithAny<WorldObject3D>();
		private readonly GL _gl;

		public EntityRenderSystem(World world, GL gl) : base(world) {
			_gl = gl;
		}

		public override void Update(in ShaderProgram shader, in Camera3D camera) {
			var query = World.Query(in _entityQuery);

			foreach(ref var chunk in query) {
				var o3ds = chunk.GetSpan<WorldObject3D>();

				foreach(var i in chunk) {
					ref var o3d = ref o3ds[i];
					o3d.Object.Render(shader);
				}
			}
		}
	}
}