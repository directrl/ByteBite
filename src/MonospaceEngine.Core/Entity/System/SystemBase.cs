using Arch.Core;

namespace MonospaceEngine.Entity.System {
	
	public abstract class SystemBase<T> {
		
		public World World { get; }

		protected SystemBase(World world) {
			World = world;
		}

		public abstract void Update(in T param);
	}
	
	public abstract class SystemBase<T1, T2> {
		
		public World World { get; }

		protected SystemBase(World world) {
			World = world;
		}

		public abstract void Update(in T1 param1, in T2 param2);
	}
}