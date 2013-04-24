namespace DummyObjects.Extensions {

	public static class object_extensions {
		public static HtmlBuilder to_html( this object obj ) {
			return new HtmlBuilder( obj );
		}
	}

	public static class string_extensions {
		public static string encode_to_aes( this string str, string key ) {
			return AESEncryption.Encrypt( str, key );
		}

		public static string decode_from_aes( this string str, string key ) {
			return AESEncryption.Decrypt( str, key );
		}
	}

}