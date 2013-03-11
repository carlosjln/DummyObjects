using System.Collections.Generic;
using Machine.Specifications;
using developwithpassion.specifications.rhinomocks;

namespace DummyObjects.UnitTests {

	[Subject(typeof (OptionList))]
	public class OptionListSpecs {

		public abstract class concern : Observes{}

		public class when_getting_the_items_of_an_option_list : concern {
			Establish context = ( ) => {
				letters_items = new List<Leters> { Leters.A, Leters.B, Leters.C };
			};

			Because of = ( ) => result = OptionList<Leters>.GetItems();

			It should_contain_the_same_items_as_defined_in_the_letter_class = ( ) => result.ShouldContain( letters_items );

			static IEnumerable<Leters> letters_items;
			static IEnumerable<Leters> result;
		}

		public class Leters : OptionList {
			public static Leters A = new Leters{Name = "A", Value = "1"};
			public static Leters B = new Leters{Name = "B", Value = "2"};
			public static Leters C = new Leters{Name = "C", Value = "3"};
		}
	}

}