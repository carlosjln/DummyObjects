using System;
using System.Collections.Generic;
using System.Reflection;

namespace DummyObjects {

	public abstract class OptionList : IOptionList {
		public class Option {
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
		}
		
		public IEnumerable<Option> Options { get{ return option_list_options; } }

		List<Option> option_list_options;
		readonly List<OptionList> option_lists = new List<OptionList>();

		protected OptionList() {
			get_options();
			option_lists.Add( this );
		}
		
		void get_options(){
			option_list_options = new List<Option>();

			var type = GetType( );
			var properties = type.GetProperties( BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly );
			var instance = Activator.CreateInstance(type);
			var item_type = typeof( Option );

			foreach ( var property_info in properties ) {
				if( property_info.PropertyType == item_type ) continue;
				
				var item = property_info.GetValue( instance, null ) as Option;
				option_list_options.Add( item );
			}
		}

//		public static T Parse<T>(string value) where T : OptionList {
//			var item = GetAll<T>().FirstOrDefault(x => x != null && x.Value.ToUpper().Equals(value.ToUpper()));
//			
//			if (item == null){
//				throw new Exception( "'"+ value +"' is not a valid '"+ typeof(T) +"'" );
//			}
//
//			return item;
//		}
//
//		// TODO: find a better way in order to avoid this code duplication
//		public static IEnumerable<OptionList> GetAll( Type type ) {
//			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly );
//			foreach (var field_info in fields){
//				var instance = Activator.CreateInstance(type);
//				yield return (OptionList) field_info.GetValue(instance);
//			}
//			
//			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly ).Where(x=>x.PropertyType == type);
//			foreach (var property_info in properties){
//				var instance = Activator.CreateInstance(type);
//				yield return (OptionList)  property_info.GetValue(instance,null);
//			}
//		}

	}

}