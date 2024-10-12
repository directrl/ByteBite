using Monospace.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace Monospace.Graphics {
	
	public class TexturedMesh : Mesh {
		
		public TexturedMesh(GL gl, PrimitiveType type,
		                    float[] vertices, float[] texCoords, int[] indices)
			: base(gl, type) {
			
			VertexCount = (uint) indices.Length;

			VAO = new(gl);
			VAO.Bind();

		#region Vertices
			var vbo = new VertexBufferObject(gl, BufferTargetARB.ArrayBuffer);
			vbos.Add(vbo);
			
			vbo.Bind();
			vbo.Data<float>(vertices, BufferUsageARB.StaticDraw);
			
			gl.EnableVertexAttribArray(0);
			gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 0, 0);
		#endregion

		#region Texture Coordinates
			vbo = new VertexBufferObject(gl, BufferTargetARB.ArrayBuffer);
			vbos.Add(vbo);
			
			vbo.Bind();
			vbo.Data<float>(texCoords, BufferUsageARB.StaticDraw);
			
			gl.EnableVertexAttribArray(1);
			gl.VertexAttribPointer(1, 2, GLEnum.Float, false, 0, 0);
		#endregion

		#region Indices
			vbo = new VertexBufferObject(gl, BufferTargetARB.ElementArrayBuffer);
			vbos.Add(vbo);
			
			vbo.Bind();
			vbo.Data<int>(indices, BufferUsageARB.StaticDraw);
		#endregion
			
			gl.BindBuffer(GLEnum.ArrayBuffer, 0);
			gl.BindVertexArray(0);
		}

		public override unsafe void Render() {
			VAO.Bind();
			gl.DrawElements(Type, VertexCount, GLEnum.UnsignedInt, null);
		}
	}
}