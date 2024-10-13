using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Utilities;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics {
	
	public class Mesh : IDisposable {

		private readonly GL _gl;
		private readonly List<VertexBufferObject> _vbos = new();
		
		public uint VertexCount { get; protected init; }
		public VertexArrayObject VAO { get; protected init; }
		
		public PrimitiveType Type { get; }
		
		public unsafe Mesh(PrimitiveType type,
		            float[] vertices, float[] normals, float[] texCoords, uint[] indices) {
			
			_gl = GLManager.Current;
			Type = type;
			
			VertexCount = (uint) indices.Length;

			VAO = new(_gl);
			VAO.Bind();

		#region Vertices
			var vbo = new VertexBufferObject(_gl, BufferTargetARB.ArrayBuffer);
			_vbos.Add(vbo);
			
			vbo.Bind();
			vbo.Data<float>(vertices, BufferUsageARB.StaticDraw);
			
			_gl.EnableVertexAttribArray(0);
			_gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
		#endregion
			
		#region Normals
			vbo = new(_gl, BufferTargetARB.ArrayBuffer);
			_vbos.Add(vbo);
			
			vbo.Bind();
			vbo.Data<float>(normals, BufferUsageARB.StaticRead);
			
			_gl.EnableVertexAttribArray(2);
			_gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
		#endregion

		#region Texture Coordinates
			vbo = new(_gl, BufferTargetARB.ArrayBuffer);
			_vbos.Add(vbo);
			
			vbo.Bind();
			vbo.Data<float>(texCoords, BufferUsageARB.StaticDraw);
			
			_gl.EnableVertexAttribArray(1);
			_gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
		#endregion

		#region Indices
			vbo = new(_gl, BufferTargetARB.ElementArrayBuffer);
			_vbos.Add(vbo);
			
			vbo.Bind();
			vbo.Data<uint>(indices, BufferUsageARB.StaticDraw);
		#endregion
			
			_gl.BindBuffer(GLEnum.ArrayBuffer, 0);
			_gl.BindVertexArray(0);
		}

		public unsafe void Render() {
			VAO.Bind();
			_gl.DrawElements(Type, VertexCount, DrawElementsType.UnsignedInt, null);
		}

		public void Dispose() {
			foreach(var vbo in _vbos) {
				vbo.Dispose();
			}
			
			VAO.Dispose();
		}
	}
}