using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DummyObjects {

	public abstract class OptionList : IOptionList {
		string name;
		public string Name {
			get { return name; }
			set {
				name = value;
				if( Value == null ) Value = value;
			}
		}

		string value;
		public string Value {
			get { return value; }
			set {
				this.value = value;

				if( name == null ) name = value;
			}
		}

		public string Description { get; set; }
		
		public override string ToString() {
			return Value;
		}

		// STATIC METHODS
		static readonly Dictionary<Type, IEnumerable<OptionList>> option_lists = new Dictionary<Type, IEnumerable<OptionList>>();
		public static IEnumerable<OptionList> GetItems( Type type ) {
			if( option_lists.ContainsKey( type ) ) return option_lists[type];

			var properties = type.GetFields( BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly );
			var instance = Activator.CreateInstance(type);
			var option_list = new List<OptionList>();

			foreach ( var property_info in properties ) {
				var item = property_info.GetValue( instance ) as OptionList;
				option_list.Add( item );
			}
			
			option_lists[type] = option_list;

			return option_list;
		}
	}
	
	public class OptionList<T> : OptionList where T : OptionList {
		public static IEnumerable<T> GetItems() {
			return GetItems( typeof(T) ).Cast<T>();
		}
	}

}