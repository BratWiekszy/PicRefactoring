using System;
using System.IO;
using System.IO.Abstractions;
using PicRefactoring.Abstractions;
using FileWrapper = PicRefactoring.Commanding.FileWrapper;

namespace PicRefactoring
{
	public class CommandsExecutor
	{
		private IFileSystem  _fileSystem;
		private ICommands    _commands;
		private IExecution[] _executions;

		public CommandsExecutor(ICommands commands) : this(new FileSystem(), commands)
		{
		}

		public CommandsExecutor(IFileSystem fileSystem, ICommands commands)
		{
			_fileSystem = fileSystem;
			_commands = commands;
		}

		public void ExecuteCommands()
		{
			var dirs = _commands.Directories;
			_executions = _commands.GetExecutions();
			foreach (var dir in dirs)
			{
				if (_fileSystem.Directory.Exists(dir) == false)
				{
					Console.WriteLine($"* Directory {dir} doesn't exist");
					continue;
				}
				Console.WriteLine($"* Processing Directory {dir}:");
				ExecuteOnDirectory(dir);
				Console.WriteLine();
			}

		}

		private void ExecuteOnDirectory(string directoryPath)
		{
			foreach (var fileName in _fileSystem.Directory.EnumerateFiles(directoryPath))
			{
				var filePath = Path.Combine(directoryPath, fileName);
				var file = _fileSystem.FileInfo.FromFileName(filePath);
				var fileWrapper = new FileWrapper(file, _fileSystem);
				ExecuteOnFile(fileWrapper);
			}
		}

		private void ExecuteOnFile(IFileWrapper file)
		{
			Console.WriteLine($"--- processing File {file.GetFileNameWithExtension()}");
			foreach (var execution in _executions)
			{
				Console.WriteLine($"----- executing {execution.Title}");
				Console.Write("     ");
				if(execution.ExecuteAction(file) == false)
					Console.WriteLine($"~ predicates not met for file {file.GetFileNameWithExtension()}");
			}
		}
	}
}
