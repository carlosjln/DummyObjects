using System;

namespace DummyObjects.Extensions {

	static class internal_extensions {

		// INHERITANCE
		static readonly Type type_of_object = typeof( object );

		public static bool inherits_from( this object reference, Type type ) {
			if( reference == null ) return false;

			return inherits_from( reference.GetType(), type );
		}

		public static bool inherits_from( this Type reference_type, Type type ) {
			if( reference_type == null || reference_type.BaseType == null) return false;
			
			var base_type = reference_type.BaseType;
			
			while( base_type != type_of_object ) {
				if( base_type == type  || (base_type.IsGenericType && base_type.GetGenericTypeDefinition() == type) ) {
					return true;
				}

				base_type = base_type.BaseType;
			}

			return false;
		}
	}

}