using System;
using System.Collections.Generic;
using DummyObjects.Extensions;

namespace DummyObjects {
	
	public class HtmlBuilder {
		static readonly Dictionary<Type, Func<object,string> > html_object_builders = new Dictionary<Type, Func<object, string>>();

		static readonly Dictionary<Type, Func<Type,string> > html_type_builders = new Dictionary<Type, Func<Type, string>>() {
			{ typeof(Enumerable), x => {
				var opening_tag = "<select>";
				var closing_tag = "</select>";

				var options = Enumerable.GetAll( x );
				var options_tag = "";

				foreach( var option in options ) {
					options_tag += "<option value=\""+ option.Value +"\">"+ option.Value +"</option>";
				}

				return opening_tag + options_tag + closing_tag;
			}}
		};

		readonly object src_object;
		readonly Type object_type;
		readonly bool is_type_only;

		public HtmlBuilder( Type object_type ) {
			is_type_only = true;
			this.object_type = object_type;
		}

		public HtmlBuilder( object src_object ) {
			this.src_object = src_object;
			object_type = src_object.GetType();
		}

		public override string ToString() {
			return Build();
		}

		public string Build() {
			return is_type_only ? build_html_from_type() : build_html_from_object();
		}

		public static void RegisterBuilder( Type object_type, Func<object, string> builder ) {
			// Direct key value assignment is used so that the previous values can be overridden
			html_object_builders[object_type] = builder;
		}

		public static void RegisterBuilder( Type object_type, Func<Type, string> builder ) {
			// Direct key value assignment is used so that the previous values can be overridden
			html_type_builders[object_type] = builder;
		}

		// TODO: This could use some sort of generic implementation to avoid all the questioning
		protected string build_html_from_object() {
			if( html_object_builders.ContainsKey( object_type ) ) return html_object_builders[object_type]( src_object );
			
			foreach( var item in html_object_builders ) {
				var registered_builder_type = item.Key;
				var builder = item.Value;

				if( object_type.inherits_from( registered_builder_type ) == false) continue;

				// Registers the builder to its direct type in order to avoid furder looping
				html_object_builders[object_type] = builder;

				return builder( src_object );
			}

			return null;
		}
		
		protected string build_html_from_type() {
			if( html_type_builders.ContainsKey( object_type ) ) return html_type_builders[object_type]( object_type );
			
			foreach( var item in html_type_builders ) {
				var registered_builder_type = item.Key;
				var builder = item.Value;

				if( object_type.inherits_from( registered_builder_type ) == false) continue;

				// Registers the builder to its direct type in order to avoid furder looping
				html_type_builders[object_type] = builder;

				return builder( object_type );
			}

			return null;
		}
	}
	
	public class HtmlBuilder<TObject> : HtmlBuilder {
		public HtmlBuilder( ) : base( typeof(TObject) ) {}
	}


//	NOTE: This classes are commented, do NOT remove, they will be used later

//	public class HtmlBuilderDslChain {
//		readonly object from_object;
//		readonly Dictionary<Type, Func<object, string>> html_builders;
//
//		readonly HtmlTag html_tag;
//
//		public HtmlBuilderDslChain( object from_object, Dictionary<Type, Func<object, string>> html_builders ) {
//			this.from_object = from_object;
//			this.html_builders = html_builders;
//		}
//
//		public string from( object obj ) {
//			throw new NotImplementedException( );
//		}
//
//		public void from<TObject>() {
//			throw new NotImplementedException();
//		}
//
//		public override string ToString() {
//			throw new NotImplementedException( );
//		}
//	}

//	class HtmlTag : Enumerable {
//		public static HtmlTag Select = new HtmlTag{ Value = "Select" };
//	}

}