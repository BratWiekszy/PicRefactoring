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
		}

		public int Order => 100000;

		public void Prepare()
		{
			_details.Prepare();
		}

		public bool FileMatches([NotNull] IFileWrapper file)
		{
			return _details.FileMatches(file);
		}
	}
}
