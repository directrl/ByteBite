using System.Numerics;

namespace MonospaceEngine.Graphics {
	
	class Normals {

		private static Vector3 ComputeFaceNormal(Vector3 p1, Vector3 p2, Vector3 p3) {
			var a = p3 - p2;
			var b = p1 - p2;

			return Vector3.Normalize(Vector3.Cross(a, b));
		}

		public static float[] CalculateNormals(float[] vertices, int[] indices) {
			var normals = new float[vertices.Length];

			for(int i = 0; i < indices.Length; i += 3) {
				var aX = vertices[indices[i] * 3];
				var aY = vertices[indices[i] * 3 + 1];
				var aZ = vertices[indices[i] * 3 + 2];
				var a = new Vector3(aX, aY, aZ);
				
				var bX = vertices[indices[i + 1] * 3];
				var bY = vertices[indices[i + 1] * 3 + 1];
				var bZ = vertices[indices[i + 1] * 3 + 2];
				var b = new Vector3(bX, bY, bZ);
				
				var cX = vertices[indices[i + 2] * 3];
				var cY = vertices[indices[i + 2] * 3 + 1];
				var cZ = vertices[indices[i + 2] * 3 + 2];
				var c = new Vector3(cX, cY, cZ);

				var normal = ComputeFaceNormal(a, b, c);
				normal = Vector3.Normalize(normal);

				normals[indices[i] * 3] = normal.X;
				normals[indices[i] * 3 + 1] = normal.Y;
				normals[indices[i] * 3 + 2] = normal.Z;
				
				normals[indices[i + 1] * 3] = normal.X;
				normals[indices[i + 1] * 3 + 1] = normal.Y;
				normals[indices[i + 1] * 3 + 2] = normal.Z;
				
				normals[indices[i + 2] * 3] = normal.X;
				normals[indices[i + 2] * 3 + 1] = normal.Y;
				normals[indices[i + 2] * 3 + 2] = normal.Z;
			}

			return normals;
		}
	}
}