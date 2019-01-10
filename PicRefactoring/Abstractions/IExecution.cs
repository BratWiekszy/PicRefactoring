using JetBrains.Annotations;

namespace PicRefactoring.Abstractions
{
	public interface IExecution
	{
		string Title { get; }

		bool ExecuteAction([NotNull] IFileWrapper file);
	}
}
