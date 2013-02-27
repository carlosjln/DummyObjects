using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DummyObjects {

	public abstract class Enumerable : IEnumerable {
		public virtual string Value { get; set; }
		public virtual string Description { get; set; }

		public static T Parse<T>(string value) where T : Enumerable {
			var item = GetAll<T>().FirstOrDefault(x => x != null && x.Value.ToUpper().Equals(value.ToUpper()));

			if (item == null){
				throw new Exception( "'"+ value +"' is not a valid '"+ typeof(T) +"'" );
			}

			return item;
		}

		public static IEnumerable<T> GetAll<T>() where T : Enumerable {
			var type = typeof (T);
			
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly );
			foreach (var field_info in fields){
				var instance = Activator.CreateInstance(type);
				yield return field_info.GetValue(instance) as T;
			}
			
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly ).Where(x=>x.PropertyType == type);
			foreach (var property_info in properties){
				var instance = Activator.CreateInstance(type);
				yield return property_info.GetValue(instance,null) as T;
			}
		}

		// TODO: find a better way in order to avoid this code duplication
		public static IEnumerable<Enumerable> GetAll( Type type ) {
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly );
			foreach (var field_info in fields){
				var instance = Activator.CreateInstance(type);
				yield return (Enumerable) field_info.GetValue(instance);
			}
			
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly ).Where(x=>x.PropertyType == type);
			foreach (var property_info in properties){
				var instance = Activator.CreateInstance(type);
				yield return (Enumerable)  property_info.GetValue(instance,null);
			}
		}

		public override string ToString() {
			return Value;
		}
	}

}