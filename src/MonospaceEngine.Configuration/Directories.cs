namespace MonospaceEngine.Configuration {
	
	public class Directories {

		public static DirectoryInfo DataRoot { get; private set; }
		public static DirectoryInfo ConfigRoot { get; private set; }

		public static void Resolve(string subName) {
			string? systemDataRoot = null;

			if(OperatingSystem.IsWindows()) {
				var appdata = Environment.GetEnvironmentVariable("APPDATA");
				if(!string.IsNullOrEmpty(appdata)) systemDataRoot = appdata;
			} else if(OperatingSystem.IsMacOS()) {
				var home = Environment.GetEnvironmentVariable("HOME");

				if(!string.IsNullOrEmpty(home)) {
					systemDataRoot = Path.Combine(
						home, "Library", "Application Support");
				}
			} else if(OperatingSystem.IsLinux()) {
				var data = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

				if(string.IsNullOrEmpty(data)) {
					data = Environment.GetEnvironmentVariable("HOME");

					if(!string.IsNullOrEmpty(data)) {
						systemDataRoot = Path.Combine(data, ".local", "share");
					}
				} else {
					systemDataRoot = data;
				}
			}

			if(string.IsNullOrEmpty(systemDataRoot)) {
				throw new SystemException("How");
			}

			DataRoot = Directory.CreateDirectory(Path.Combine(systemDataRoot,
				EngineSettings.BASE_NAME, subName));
			ConfigRoot = Directory.CreateDirectory(Path.Combine(DataRoot.FullName, "config"));
		}
	}
}