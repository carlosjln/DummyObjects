namespace DummyObjects {
	internal class test {
		public test() {
			var x = OptionList<Leters>.Items;
			var x = OptionList.GetOptions;
		}
	}

	internal class Leters : OptionList {
		public static Leters A = new Leters{Name = "A", Value = "1"};
		public static Leters B = new Leters{Name = "B", Value = "2"};
		public static Leters C = new Leters{Name = "C", Value = "3"};
	}

}