using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Quark.Utility.Timers;
using Quark.Graphics.Scene;
using Silk.NET.Core.Contexts;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Quark.Graphics.Windowing {

	public abstract unsafe class WindowBase<THandle> {
		
		public THandle* Handle { get; init; }
		
		protected IGLContext? GLContext { get; init; }
		protected GL? GL { get; init; }

		protected bool Current { get; set; } = false;

		public DeltaTimer Timer { get; } = new();
		public IScene? Scene { get; set; }

		public virtual string Title { get; set; } = "Window";
		public virtual Vector2D<int> Dimensions { get; set; } = new(512, 512);
		public virtual Vector2D<int> Position { get; set; } = new(0, 0);

		public abstract void Show();
		public abstract void Hide();

		public virtual void Begin() {
			Timer.Start();
		}

		public virtual void End() {
			Timer.Reset();
		}

		public void Update() {
			Scene?.Update(Timer.DeltaTime);
		}

		public void Render() {
			Debug.Assert(GL != null);
			Scene?.Render(this, GL);
		}
	}
}