using System.Diagnostics;
using MonospaceEngine.Configuration;
using MonospaceEngine.Graphics.OpenGL;
using MonospaceEngine.Logging;
using MonospaceEngine.Utilities;
using Serilog;
using Serilog.Core;
using Window = MonospaceEngine.Graphics.Window;

namespace MonospaceEngine {
	
	public sealed class Application {

		internal static Monospace? _currentApplication;
		public static Monospace? CurrentApplication => _currentApplication;
	}
	
	public class Monospace {
		
		public string Id { get; private set; }
		public bool Running { get; protected set; }
		
		internal static Logger EngineLogger { get; private set; }
		public static Logger? AppLogger { get; protected set; }

		public static List<Window> Windows { get; private set; }
		public static GameSettings GameSettings { get; private set; }

		public static Resources EngineResources { get; private set; }
		public static Resources AppResources { get; protected set; }

		protected Monospace(string id) {
			Application._currentApplication = this;
			Id = id;

			Directories.Resolve(id);

			Windows = new();
			GameSettings = new(new FileInfo(Path.Combine(
				Directories.ConfigRoot.FullName, "default.json")));

			EngineResources = new("MonospaceEngine.Assets");
		}
		
		public virtual void Initialize() { }
		public virtual void Update() { }

		public void Start(string[] args) {
			var config = LoggerFactory.CreateDefaultConfiugration(LoggerPurpose.Engine);
			
			if(args.Contains("--debug")) {
				Debugging.DebugMode = true;
				config.MinimumLevel.Debug();
			} else {
				config.MinimumLevel.Information();
			}

			EngineLogger = config.CreateLogger();
			Log.Logger = EngineLogger;
			EngineLogger.Information($"Debugging enabled: {Debugging.DebugMode}");
			
			Initialize();
			Running = true;
			
			Debug.Assert(Windows.Count > 0);

			var primaryWindow = Windows.First();
			while(!primaryWindow.Impl.IsClosing && Running) {
				foreach(var window in Windows) {
					if(window.GL != null) {
						GLManager.Current = window.GL;
					}
					
					window.Impl.DoEvents();
					
					if(!window.Impl.IsClosing) window.Impl.DoUpdate();
					if(window.Impl.IsClosing) continue;
					
					window.Impl.DoRender();
				}
			}
			
			Shutdown();
			
			if(Debugging.DebugMode) {
				// Console.WriteLine("Debugging is enabled; press ESC to exit");
				// while(Console.ReadKey(false).Key != ConsoleKey.Escape) {
				// 	Thread.Sleep(100);
				// }
				Thread.Sleep(100);
			}
		}

		public void Shutdown() {
			EngineLogger.Information("Shutting down");
			Running = false;

			foreach(var window in Windows) {
				window.Dispose();
			}
			
			GameSettings.Save();
		}
	}
}