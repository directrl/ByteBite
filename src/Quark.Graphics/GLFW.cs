using System.Diagnostics.CodeAnalysis;
using Silk.NET.GLFW;

namespace Quark.Graphics {
	
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
			byte* rawDescription;
			var error = GLFW.API.GetError(out rawDescription);
				
			string description = "";
			if(rawDescription != null) description = new string((sbyte*) rawDescription);

			return $"{error.ToString()} - {description}";
		}
	}
}