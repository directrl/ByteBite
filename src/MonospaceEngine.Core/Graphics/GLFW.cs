using Silk.NET.GLFW;

namespace MonospaceEngine.Graphics {
	
	public unsafe class GLFW {

		private static Glfw? _api;

		public static Glfw API {
			get {
				if(_api == null) {
					_api = Glfw.GetApi();
				}

				return _api;
			}
		}

		public static string GetErrorString() {
			var error = API.GetError(out byte* rawDescription);
			
			string description = "";
			if(rawDescription != null) description = new string((sbyte*) rawDescription);

			return $"{error.ToString()} - {description}";
		}
	}
}