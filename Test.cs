namespace DummyObjects {
	internal class test {
		public test() {
			var x = Leters.GetItems();
		}
	}

	internal class Leters : OptionList {
		public static Option A = new Option{Name = "A", Value = "1"};
		public static Option B = new Option{Name = "B", Value = "2"};
		public static Option C = new Option{Name = "C", Value = "3"};
	}

}