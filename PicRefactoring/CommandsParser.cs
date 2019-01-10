﻿using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace PicRefactoring
{
	public class CommandsParser
	{
		public const string CommandsFileName = "commands.json";

		private FileInfoBase _commandFile;
		private IFileSystem _fileSystem;

		public CommandsParser(IFileSystem fileSystem) 
		{
			_fileSystem = fileSystem;
		}

		public CommandsParser() : this(new FileSystem()) {}

		public FileInfoBase CommandFile => _commandFile;

		public void OpenCommandsFile(string[] programArgs)
		{
			var path = @".\commands.json";
			if (ContainsFilePath(programArgs))
			{
				path = programArgs.First();
				if (path.EndsWith(".json") == false)
					path = Path.Combine(path, CommandsFileName);
			}
			if(_fileSystem.File.Exists(path) == false)
				throw new FileNotFoundException(CommandsFileName);

			_commandFile = _fileSystem.FileInfo.FromFileName(path);
		}

		private bool ContainsFilePath(string[] args)
		{
			if (args == null || args.Length != 1)
				return false;

			var path = args.First();
			if (path.IsNullOrWhitespace())
				return false;

			return true;
		}
	}
}
