using JetBrains.Annotations;

namespace PicRefactoring.Abstractions
{
	public interface ICommands 
	{
		[NotNull]
		string[] Directories { get; }

		[NotNull] 
		IExecution[] GetExecutions();
	}
}