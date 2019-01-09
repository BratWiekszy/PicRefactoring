using JetBrains.Annotations;
using PicRefactoring.Abstractions;

namespace PicRefactoring.Actions
{
	public interface IFileAction
	{
		void CheckValidity();

		void ActOnFile([NotNull] IFileWrapper file);
	}
}
