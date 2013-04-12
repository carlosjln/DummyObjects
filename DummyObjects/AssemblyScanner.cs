using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using DummyObjects.Extensions;

namespace DummyObjects {

	public class AssemblyScanner {
		readonly static AssemblyScanner singleton_instance = new AssemblyScanner( );
		readonly static IEnumerable<Assembly> assemblies = load_assemblies();

		/// <summary>
		/// Gets all inheritors of the specified type.
		/// </summary>
		public List<Type> GetAllInheritors( Type type ) {
			var return_types = new List<Type>( );
			var generic_type_definition = type.IsGenericType ? type.GetGenericTypeDefinition( ) : null;

			foreach( var assembly in assemblies ) {
				// This try-catch is required because it can't get all types located in some assemblies (eg: mscorlib)
				try {
					var types = assembly.GetTypes( );

					foreach( var item in types ) {
						if( !item.IsClass || item.IsAbstract ) continue;
						if( item.inherits_from( type ) || ( generic_type_definition != null && item.inherits_from( generic_type_definition ) ) ) return_types.Add( item );
					}
				} catch( Exception ) {
					// No exception is thrown because if it cant load all types withing the assembly it might not matter anyways.
				}
			}

			return return_types;
		}

		public List<Assembly> GetAllAssembliesContaining( Type type ) {
			var assembly_list = new List<Assembly>( );
			
			foreach( var assembly in assemblies ) {
				var types = assembly.GetTypes( );
				foreach( var item in types ) {
					if( type.IsAssignableFrom( item ) ) {
						assembly_list.Add( assembly );
						break;
					}
				}
			}

			return assembly_list;
		}

		static IEnumerable<Assembly> load_assemblies() {
			foreach( var assembly_file in Directory.GetFiles( HttpRuntime.BinDirectory, "*.dll" ) ) {
				yield return Assembly.LoadFrom( assembly_file );
			}
		}

		public static AssemblyScanner Instance {
			get { return singleton_instance; }
		}
	}

}