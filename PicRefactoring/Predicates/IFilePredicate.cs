using JetBrains.Annotations;
using PicRefactoring.Abstractions;

namespace PicRefactoring.Predicates
{
	public interface IFilePredicate
	{
		bool FileMatches([NotNull] IFileWrapper file);
	}
}
