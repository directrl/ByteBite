using System.Diagnostics;

namespace Quark.Utility.Timers {
	
	public class DeltaTimer : ITimer {

		private Stopwatch sw = new();

		public double DeltaTime => sw.Elapsed.TotalMilliseconds;

		public void Start() {
			sw.Start();
		}

		public void Stop() {
			sw.Stop();
		}

		public void Reset() {
			sw.Reset();
		}
	}
}