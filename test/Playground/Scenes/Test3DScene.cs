using System.Numerics;
using MonospaceEngine.Graphics;
using MonospaceEngine.Graphics._3D;
using MonospaceEngine.Graphics._3D.Camera;
using MonospaceEngine.Graphics.Scene;
using Silk.NET.Input;
using Silk.NET.OpenGL;

namespace Playground.Scenes {
	
	public class Test3DScene : Scene3D {

		private Mesh? Mesh;
		private Model3D? Model;

		private bool CursorToggle = true;

		public Test3DScene() : base("test3d") { }

		public override void OnLoad(Window window) {
			base.OnLoad(window);

			Camera = new PerspectiveCamera(window) {
				FOV = 60.0f
			};
			
			Mesh = new ColoredMesh(window.GL, PrimitiveType.Triangles,
				new float[] {
					// VO
					-0.5f, 0.5f, 0.5f,
					// V1
					-0.5f, -0.5f, 0.5f,
					// V2
					0.5f, -0.5f, 0.5f,
					// V3
					0.5f, 0.5f, 0.5f,
					// V4
					-0.5f, 0.5f, -0.5f,
					// V5
					0.5f, 0.5f, -0.5f,
					// V6
					-0.5f, -0.5f, -0.5f,
					// V7
					0.5f, -0.5f, -0.5f,
				},
				new byte[] {
					127, 0, 0,
					0, 127, 0,
					0, 0, 127,
					0, 127, 127,
					127, 0, 0,
					0, 127, 0,
					0, 0, 127,
					0, 127, 127
				},
				new int[] {
					// Front face
					0, 1, 3, 3, 1, 2,
					// Top Face
					4, 0, 3, 5, 4, 3,
					// Right face
					3, 2, 7, 5, 3, 7,
					// Left face
					6, 1, 0, 6, 0, 4,
					// Bottom face
					2, 1, 6, 2, 6, 7,
					// Back face
					7, 6, 4, 7, 4, 5,
				}
			);

			Model = new("quad", Mesh);
			Mouse.MouseMove += CameraMove;
		}
		
		public override void Update(double delta) {
			if(Keyboard?.IsKeyPressed(Key.Escape) ?? false) {
				CursorToggle = !CursorToggle;
			}

			if(Mouse != null) {
				Mouse.Cursor.CursorMode = CursorToggle
					? CursorMode.Disabled
					: CursorMode.Normal;
			}
			
			var fDelta = (float) delta;
			
			Camera.Position.Z += 0.3f * fDelta;
			Model.Position.Y += 0.12f * (float)delta;
		}

		public override void Render(GL gl) {
			base.Render(gl);

			MainShader.SetUniform("model", Model.ModelMatrix);
			Model?.Render(gl);
		}

		Vector2 LastMousePosition;
		const float CameraSensitivity = 0.1f;
		void CameraMove(IMouse mouse, Vector2 mousePosition) {
			if(!CursorToggle) return;
			
			if(LastMousePosition == default) {
				LastMousePosition = mousePosition;
			} else {
				var deltaX = (mousePosition.X - LastMousePosition.X) * CameraSensitivity;
				var deltaY = (mousePosition.Y - LastMousePosition.Y) * CameraSensitivity;
				LastMousePosition = mousePosition;

				Camera.Yaw += deltaX;
				Camera.Pitch -= deltaY;

				Camera.Pitch = Math.Clamp(Camera.Pitch, -89.9f, 89.9f);
			}
		}
	}
}