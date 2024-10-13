using System.Text;
using Serilog;
using Silk.NET.OpenGL;

namespace MonospaceEngine.Utilities {
	
	public static class GLManager {

		public static GL Current { get; set; }
		
		public unsafe static void EnableDefaults() {
			Current.Enable(EnableCap.DepthTest);
			Current.Enable(EnableCap.CullFace);
			
			Current.CullFace(TriangleFace.Back);

			if(Debugging.DebugMode) {
				Current.Enable(EnableCap.DebugOutput);
				Current.Enable(EnableCap.DebugOutputSynchronous);
				Current.DebugMessageCallback(GlDebugPrint, null);
				Current.DebugMessageControl(
					DebugSource.DontCare,
					DebugType.DontCare,
					DebugSeverity.DontCare,
					0, null,
					true
				);
			}
		}

		public static bool CheckError() {
			var error = Current.GetError();

			switch(error) {
				case GLEnum.NoError:
					return false;
				default:
					Log.Error($"OpenGL Error: {error}");
					return true;
			}
		}
		
		private unsafe static void GlDebugPrint(GLEnum source,
		                                        GLEnum type,
		                                        int id,
		                                        GLEnum severity,
		                                        int length,
		                                        nint message,
		                                        nint param) {

			StringBuilder msg = new StringBuilder("OpenGL Debug Message ");
			msg.Append(id);
			msg.Append('\n');
			msg.Append('\t');

			switch(severity) {
				case GLEnum.DebugSeverityHigh:
					msg.Append("(HIGH)");
					break;
				case GLEnum.DebugSeverityMedium:
					msg.Append("(MEDIUM)");
					break;
				case GLEnum.DebugSeverityLow:
					msg.Append("(LOW)");
					break;
				case GLEnum.DebugSeverityNotification:
					msg.Append("(NOTIFY)");
					break;
			}

			msg.Append(' ');

			switch(type) {
				case GLEnum.DebugTypeError:
					msg.Append("ERROR");
					break;
				case GLEnum.DebugTypeDeprecatedBehavior:
					msg.Append("Deprecated");
					break;
				case GLEnum.DebugTypeUndefinedBehavior:
					msg.Append("Undefined");
					break;
				case GLEnum.DebugTypePortability:
					msg.Append("Portability");
					break;
				case GLEnum.DebugTypePerformance:
					msg.Append("Performance");
					break;
				case GLEnum.DebugTypeMarker:
					msg.Append("Marker");
					break;
				case GLEnum.DebugTypePushGroup:
					msg.Append("Push Group");
					break;
				case GLEnum.DebugTypePopGroup:
					msg.Append("Pop Group");
					break;
				case GLEnum.DebugTypeOther:
					msg.Append("Other");
					break;
			}

			msg.Append('\n');
			msg.Append("\tSource: ");

			switch(source) {
				case GLEnum.DebugSourceApi:
					msg.Append("API");
					break;
				case GLEnum.DebugSourceWindowSystem:
					msg.Append("Window System");
					break;
				case GLEnum.ShaderCompiler:
					msg.Append("Shader Compiler");
					break;
				case GLEnum.DebugSourceThirdParty:
					msg.Append("Third Party");
					break;
				case GLEnum.DebugSourceApplication:
					msg.Append("Application");
					break;
				case GLEnum.DebugSourceOther:
					msg.Append("Other");
					break;
			}

			msg.Append('\n');
			msg.Append("\tMessage: ");

			var m = new string((sbyte*) message, 0, length, Encoding.UTF8);
			msg.Append(m);
			
			Log.Debug(msg.ToString());
		}
	}
}