using System;

namespace PicRefactoring.Commanding
{
	public class BadCommandException : Exception
	{
		public BadCommandException() {}

		public BadCommandException(string message) 
			: base("Invalid data: "+ message) {}

		public BadCommandException(string message, BadCommandException innerException) 
			: base(message + Environment.NewLine + innerException.Message, innerException) {}
	}
}
