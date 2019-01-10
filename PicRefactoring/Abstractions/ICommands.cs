using PicRefactoring.Commanding;

namespace PicRefactoring.Abstractions
{
	public interface ICommands 
	{
		string[] Directories { get; }

		Execution[] GetExecutions();
	}
}