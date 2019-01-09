using System.IO.Abstractions;
using System.Linq;
using JetBrains.Annotations;
using PicRefactoring.Abstractions;
using PicRefactoring.Actions;
using PicRefactoring.Predicates;

namespace PicRefactoring.Commanding
{
	public class Execution
	{
		private ExecutionEntry _entry;
		private IFilePredicate[] _predicates;
		private IFileAction[]    _actions;

		public Execution([NotNull] IFilePredicate[] predicates, [NotNull] IFileAction[] actions, ExecutionEntry entry = null)
		{
			_predicates = predicates;
			_actions = actions;
			_entry = entry;
		}

		public string Title => _entry?.Title;

		public void ExecuteAction([NotNull] FileInfoBase file)
		{
			ExecuteAction(new FileWrapper(file));
		}

		public void ExecuteAction([NotNull] IFileWrapper file)
		{
			if (_predicates.All(p => p.FileMatches(file)))
			{
				foreach (var action in _actions)
				{
					action.ActOnFile(file);
				}
			}


		}
	}
}
