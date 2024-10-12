using Monospace.Graphics._3D.Camera;
using Monospace.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace Monospace.Graphics.Scene {
	
	public abstract class Scene3D : SceneBase {

		protected ShaderProgram MainShader { get; set; }
		public Camera3D? Camera { get; set; }

		public Scene3D(string id) : base(id) { }

		public override void OnLoad(Window window) {
			base.OnLoad(window);

			MainShader = new ShaderProgram(window.GL,
				new(ShaderType.FragmentShader,
					Monospace.CurrentApplication?.EngineResources[ResourceType.SHADER, "scene.frag"].ReadString()),
				new(ShaderType.VertexShader,
					Monospace.CurrentApplication?.EngineResources[ResourceType.SHADER, "scene.vert"].ReadString())
			);
		}

		public override void Render(GL gl) {
			MainShader.Bind();
			Camera?.Render(gl, MainShader);
		}
	}
}