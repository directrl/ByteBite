using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.OpenGL {
	
	public readonly struct VertexBufferObject : IDisposable {

		private readonly GL gl;
		
		public uint Id { get; }
		public BufferTargetARB Target { get; }
		
		public VertexBufferObject(GL gl, BufferTargetARB target) {
			this.gl = gl;
			Id = gl.GenBuffer();
			Target = target;
		}

		public void Bind() {
			gl.BindBuffer(Target, Id);
		}

		public unsafe void Data<TDataType>(Span<TDataType> data, BufferUsageARB usage) {
			fixed(void* d = data) {
				gl.BufferData(Target, (nuint) (data.Length * sizeof(TDataType)), d, usage);
			}
		}
		
		public void Dispose() {
			gl.DeleteBuffer(Id);
		}
	}
}