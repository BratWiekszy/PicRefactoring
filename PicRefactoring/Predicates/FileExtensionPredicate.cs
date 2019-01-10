using System;
using System.Linq;
using JetBrains.Annotations;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;

namespace PicRefactoring.Predicates
{
	public class FileExtensionPredicate : IFilePredicate
	{
		private string[] _extensions;

		public FileExtensionPredicate([NotNull] string[] extensions) 
		{
			_extensions = extensions;
		}

		public int Order => 10;

		public void Prepare()
		{
			CheckValidity();
			CleanExtensions();
		}

		private void CheckValidity()
		{
			if(_extensions.Any(e => e.IsNullOrWhitespace()))
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
