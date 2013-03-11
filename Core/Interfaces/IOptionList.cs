using System.Collections.Generic;

namespace DummyObjects {

	public interface IOptionList {
		IEnumerable<OptionList.Option> Options { get; }
	}

}