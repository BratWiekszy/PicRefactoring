using System.IO.Abstractions;
using JetBrains.Annotations;

namespace PicRefactoring.Actions
{
	public class RenameAction : IFileAction
	{
		private Rename _rename;

		public RenameAction([NotNull] Rename rename) 
		{
			_rename = rename;
		}

		public virtual void CheckValidity()
		{
			_rename.CheckValidity();
		}

		public void ActOnFile([NotNull] FileWrapper file, [NotNull] DirectoryInfoBase directory)
		{

		}

		public bool FileExists(string fileName, DirectoryBase directory)
		{
			return false;
		}
	}
}
