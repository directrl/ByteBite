using MonospaceEngine.Graphics;
using MonospaceEngine.Graphics._3D;

namespace MonospaceEngine.Entity.Component {
	
	public struct WorldObject3D {
		
		public Object3D Object;

	#region Position
		public float X {
			get => Object.Position.X;
			set => Object.Position.X = value;
		}
		
		public float Y {
			get => Object.Position.Y;
			set => Object.Position.Y = value;
		}
		
		public float Z {
			get => Object.Position.Z;
			set => Object.Position.Z = value;
		}
	#endregion

	#region Rotation
		public float Pitch {
			get => Object.Rotation.X;
			set => Object.Rotation.X = value;
		}
		
		public float Yaw {
			get => Object.Rotation.Y;
			set => Object.Rotation.Y = value;
		}
		
		public float Roll {
			get => Object.Rotation.Z;
			set => Object.Rotation.Z = value;
		}
	#endregion
		
	#region Rotation XYZ
		public float RotationX {
			get => Object.Rotation.X;
			set => Object.Rotation.X = value;
		}
		
		public float RotationY {
			get => Object.Rotation.Y;
			set => Object.Rotation.Y = value;
		}
		
		public float RotationZ {
			get => Object.Rotation.Z;
			set => Object.Rotation.Z = value;
		}
	#endregion

	#region Scale
		public float ScaleX {
			get => Object.Scale.X;
			set => Object.Scale.X = value;
		}
		
		public float ScaleY {
			get => Object.Scale.Y;
			set => Object.Scale.Y = value;
		}
		
		public float ScaleZ {
			get => Object.Scale.Z;
			set => Object.Scale.Z = value;
		}
	#endregion
	}
}