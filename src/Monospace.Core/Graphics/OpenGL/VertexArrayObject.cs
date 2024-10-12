using Silk.NET.OpenGL;

namespace Monospace.Graphics.OpenGL {
	
	public readonly struct VertexArrayObject : IDisposable {

		private readonly GL gl;
		
		public uint Id { get; }

		public VertexArrayObject(GL gl) {
			this.gl = gl;
			Id = gl.GenVertexArray();
		}

		public void Bind() {
			gl.BindVertexArray(Id);
		}

		public void Dispose() {
			gl.DeleteVertexArray(Id);
		}
	}
}