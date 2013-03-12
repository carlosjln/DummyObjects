using System.Collections.Generic;
using Machine.Specifications;
using developwithpassion.specifications.rhinomocks;

namespace DummyObjects.UnitTests {

	[Subject(typeof (OptionList))]
	public class OptionListSpecs {

		public abstract class concern : Observes{}

		public class when_getting_the_items_of_an_option_list : concern {
			Establish context = ( ) => {
				list = new List<Letters> { Letters.A, Letters.B, Letters.C };
				letters_items = OptionList<Letters>.GetItems();
			};

			It should_contain_the_same_items_as_Letter_class = ( ) => letters_items.ShouldContain( list );

			static IEnumerable<Letters> list;
			static IEnumerable<Letters> letters_items;
		}

		public class Letters : OptionList {
			public static Letters A = new Letters{Name = "A", Value = "1"};
			public static Letters B = new Letters{Name = "B", Value = "2"};
			public static Letters C = new Letters{Name = "C", Value = "3"};
		}
	}

}