using System;
using System.Collections.Generic;
using DummyObjects.Extensions;

namespace DummyObjects {
	
	public class HtmlBuilder {
		static readonly Dictionary<Type, Func<object,string> > html_object_builders = new Dictionary<Type, Func<object, string>>();

		static readonly Dictionary<Type, Func<Type,string> > html_type_builders = new Dictionary<Type, Func<Type, string>>() {
			{typeof(OptionList), x => {
				const string opening_tag = "<select>";
				const string closing_tag = "</select>";

				// TODO: consider caching the type instances
				var instance = Activator.CreateInstance(x) as OptionList;
				var options_tag = "";

			    if( instance == null ) return "";

			    foreach( var option in instance.Options ) {
			        options_tag += "<option value=\""+ option.Value +"\">"+ (option.Name ?? option.Value ) +"</option>";
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

}