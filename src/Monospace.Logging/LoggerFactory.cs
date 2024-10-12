using Monospace.Configuration;
using Serilog;
using Serilog.Core;

namespace Monospace.Logging {

	public static class LoggerFactory {

		public static LoggerConfiguration CreateDefaultConfiugration(LoggerPurpose purpose) {
			var config = new LoggerConfiguration()
			             .WriteTo.Console();

			string? path = null;
			
			switch(purpose) {
				case LoggerPurpose.Engine:
					path = Path.Combine(Directories.DataRoot.FullName, "logs", "engine.log");
					break;
				case LoggerPurpose.Application:
					path = Path.Combine(Directories.DataRoot.FullName, "logs", "app.log");
					break;
			}

			if(!string.IsNullOrEmpty(path)) {
				config.WriteTo.File(path,
					rollingInterval: RollingInterval.Day,
					retainedFileCountLimit: 10);
			}

			return config;
		}
	}
}