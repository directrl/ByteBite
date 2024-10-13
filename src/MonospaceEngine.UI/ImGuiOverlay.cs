using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace MonospaceEngine.UI {
	
	public class ImGuiOverlay : Overlay {

		public ImGuiController? Controller { get; private set; }
		private IInputContext? _input;

		public ImGuiOverlay(IWindow window) : base(window) {
			window.Load += () => {
				_input = Window.CreateInput();
				Controller = new(GL, Window, _input);
			};

			window.Closing += () => {
				Controller?.Dispose();
				_input?.Dispose();
			};
		}

		public override void OnRender(double delta) {
			Controller?.Update((float) delta);
			base.OnRender(delta);
			Controller?.Render();
		}
	}
}