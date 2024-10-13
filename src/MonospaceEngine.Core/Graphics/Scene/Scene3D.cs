using System.Numerics;
using MonospaceEngine.Graphics._3D;
using MonospaceEngine.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Graphics.Scene {
	
	public abstract class Scene3D : SceneBase {
		
		protected ShaderProgram MainShader { get; set; }
		public SceneEnvironment? Environment { get; set; }
		public Camera3D? Camera { get; set; }

		public Scene3D(string id) : base(id) { }

		public override void OnLoad(Window window) {
			base.OnLoad(window);
			
			GL = window.GL;

			MainShader = new(GL,
				new(ShaderType.FragmentShader,
					Monospace.EngineResources[ResourceType.SHADER, "material.frag"].ReadString()),
				new(ShaderType.VertexShader,
					Monospace.EngineResources[ResourceType.SHADER, "material.vert"].ReadString())
			);
			MainShader.Validate();
		}

		public override void Render() {
			MainShader.Bind();

			if(Camera != null) {
				Camera.Render(MainShader);
				MainShader.SetUniform("cameraPos", Camera.Position);
			}
			
			Environment?.Light.Render(MainShader);
		}
	}
}