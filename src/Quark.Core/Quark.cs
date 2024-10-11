namespace Quark.Core {
	
	public sealed class Quark {

		internal static ApplicationBase? _globalApplication;
		public static ApplicationBase? GlobalApplication => _globalApplication;
	}
}