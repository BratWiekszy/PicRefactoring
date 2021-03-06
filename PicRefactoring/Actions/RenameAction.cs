﻿using System.IO;
using System.IO.Abstractions;
using JetBrains.Annotations;
using PicRefactoring.Abstractions;

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

		public string ActOnFile([NotNull] IFileWrapper file)
		{
			var name = file.GetFileName();
			var ext = "."+ file.GetExtension();
			string newName = _rename.GetRenamedFileName(name);
			while(FileExists(newName + ext, file.UnderlyingFile))
			{
				newName = _rename.TryMakeFileNameUnique(newName, name);
			}

			var newFileName = newName + ext;
			file.UnderlyingFile.MoveTo(Path.Combine(file.UnderlyingFile.Directory.FullName, newFileName));
			return $"  renamed file to {newFileName}";
		}

		private bool FileExists(string fileName, FileInfoBase file)
		{
			var path = Path.Combine(file.DirectoryName, fileName);
			return file.FileSystem.File.Exists(path);
		}
	}
}
