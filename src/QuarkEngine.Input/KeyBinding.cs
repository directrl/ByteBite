namespace QuarkEngine.Input {
	
	public class KeyBinding {
		
		public string Name { get; init; }
		public int Key { get; init; }
		public int Mods { get; set; }

		internal bool down = false;
		internal bool pressed = false;
		internal bool released = false;

		public bool WasPressed() {
			var old = pressed;
			pressed = false;
			return old;
		}

		public bool WasReleased() {
			var old = released;
			released = false;
			return old;
		}

		public bool IsDown() {
			return down;
		}
	}
}