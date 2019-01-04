using JetBrains.Annotations;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;

namespace PicRefactoring.Predicates
{
	public class FilePropertiesPredicate : IFilePredicate
	{
		private PropertiesDetails _details;

		public FilePropertiesPredicate([NotNull] PropertiesDetails details) 
		{
			_details = details;
			details.Prepare();
		}

		public int Order => 100000;

		public bool FileMatches([NotNull] IFileWrapper file)
		{
			return _details.FileMatches(file);
		}
	}
}
