using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics {
	
	public abstract class Mesh : IDisposable {

		internal readonly GL gl;
		internal List<VertexBufferObject> vbos = new();
		
		public uint VertexCount { get; protected init; }
		public VertexArrayObject VAO { get; protected init; }
		
		public PrimitiveType Type { get; }
		
		protected Mesh(GL gl, PrimitiveType type) {
			this.gl = gl;
			Type = type;
		}

		public abstract void Render();

		public void Dispose() {
			foreach(var vbo in vbos) {
				vbo.Dispose();
			}
			
			VAO.Dispose();
		}
	}
}