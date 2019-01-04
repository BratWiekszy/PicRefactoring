using System;
using System.Linq;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;

namespace PicRefactoring.Predicates
{
	public class FileExtensionPredicate : IFilePredicate
	{
		private string[] _extensions;

		public FileExtensionPredicate(string[] extensions) 
		{
			_extensions = extensions;

			CheckValidity();
			CleanExtensions();
		}

		private void CheckValidity()
		{
			if(_extensions.Any(e => string.IsNullOrEmpty(e)))
				throw new BadCommandException();
		}

		private void CleanExtensions()
		{
			_extensions = _extensions.Select(e =>
			{
				if(e.StartsWith('.'))
					return e.Substring(1).ToLower();

				return e.ToLower();
			}).ToArray();
		}

		public bool FileMatches(IFileWrapper file)
		{
			var ext = file.GetExtension().ToLower();
			return _extensions.Contains(ext);
		}
	}
}
