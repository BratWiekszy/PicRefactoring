﻿using System.IO;
using System.IO.Abstractions;
using JetBrains.Annotations;
using FileWrapper = PicRefactoring.Commanding.FileWrapper;

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
			var name = file.GetFileName();
			var ext = "."+ file.GetExtension();
			string newName = _rename.GetRenamedFileName(name);
			while(FileExists(newName + ext, file.UnderlyingFile))
			{
				newName = _rename.TryMakeFileNameUnique(newName, name);
			}
			file.UnderlyingFile.MoveTo(newName + ext);
		}

		private bool FileExists(string fileName, FileInfoBase file)
		{
			var path = Path.Combine(file.DirectoryName, fileName);
			return file.FileSystem.File.Exists(path);
		}
	}
}
