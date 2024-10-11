namespace Quark.Core {
	
	public abstract class ApplicationBase {
		
		public string Id { get; init; }
		public bool Running { get; protected set; } = false;
		
		protected ApplicationBase(string id) {
			Quark._globalApplication = this;
			Id = id;
		}
		
		public virtual void Initialize() { }
		public virtual void Update() { }

		public abstract void Start(string[] args);
	}
}