using JetBrains.Annotations;
using PicRefactoring.Abstractions;

namespace PicRefactoring.Actions
{
	public interface IFileAction
	{
		void CheckValidity();

		string ActOnFile([NotNull] IFileWrapper file);
	}
}
