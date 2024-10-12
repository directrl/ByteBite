using System.Numerics;
using Monospace.Extensions;
using MonospaceEngine.Graphics.Interfaces;
using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics._3D.Camera {
	
	public abstract class Camera3D : IShaderRenderable {

		protected static float Z_NEAR = 0.01f;
		protected static float Z_FAR = 1000f;
		
		public Matrix4x4 ProjectionMatrix { get; protected set; }
		public Matrix4x4 InverseProjectionMatrix { get; protected set; }
		public Matrix4x4 ViewMatrix { get; protected set; }
		public Matrix4x4 InverseViewMatrix { get; protected set; }

		public Vector3 Position = new();

		private float _yaw;
		public float Yaw {
			get => _yaw;
			set {
				_yaw = value;
				Direction.X = MathF.Cos(value.ToRadians()) * MathF.Cos(_pitch.ToRadians());
				Direction.Z = MathF.Sin(value.ToRadians()) * MathF.Cos(_pitch.ToRadians());
				Front = Vector3.Normalize(Direction);
			}
		}
		
		private float _pitch;
		public float Pitch {
			get => _pitch;
			set {
				_pitch = value;
				Direction.X = MathF.Cos(_yaw.ToRadians()) * MathF.Cos(value.ToRadians());
				Direction.Y = MathF.Sin(value.ToRadians());
				Direction.Z = MathF.Sin(_yaw.ToRadians()) * MathF.Cos(value.ToRadians());
				Front = Vector3.Normalize(Direction);
			}
		}

		private Vector3 Direction = new();
		private Vector3 Front = new(0.0f, 0.0f, -1.0f);
		private Vector3 Up = Vector3.UnitY;

		private float _fov = 1.0f;
		public float FOV {
			get => _fov;
			set {
				_fov = value;
				RecalculateProjectionMatrix();
			}
		}
		
		protected float Width { get; private set; }
		protected float Height { get; private set; }

		protected Camera3D(Window window) {
			Width = window.Impl.FramebufferSize.X;
			Height = window.Impl.FramebufferSize.Y;
			
			RecalculateProjectionMatrix();
			RecalculateViewMatrix();

			window.Impl.FramebufferResize += size => {
				Width = size.X;
				Height = size.Y;
				
				RecalculateProjectionMatrix();
			};
		}

		public void Render(GL gl, ShaderProgram program) {
			RecalculateViewMatrix();
			
			program.SetUniform("projection", ProjectionMatrix);
			program.SetUniform("view", ViewMatrix);
		}

		protected abstract void RecalculateProjectionMatrix();

		protected void RecalculateViewMatrix() {
			ViewMatrix = Matrix4x4.CreateLookAt(
				Position,
				Position + Front,
				Up
			);
			
			Matrix4x4.Invert(ViewMatrix, out var ivm);
			InverseViewMatrix = ivm;
		}
	}
}