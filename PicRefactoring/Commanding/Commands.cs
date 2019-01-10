using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using JsonRazor.Serialization;
using PicRefactoring.Abstractions;

namespace PicRefactoring.Commanding
{
	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public class Commands : ICommands
	{
		private IExecution[] _executions;

		public string[]         Directories { get; private set; }
		public ExecutionEntry[] Executions  { get; private set; }

		private Commands() {}

		public Commands(string[] directories, ExecutionEntry[] executions)
		{
			Directories = directories;
			Executions = executions;
		}

		public void CheckValidity()
		{
			if(Directories == null || Directories.Length == 0)
				throw new BadCommandException("no directories");

			Directories.All(d => CheckPathValidity(d));

			if(Executions == null || Executions.Length == 0)
				throw new BadCommandException("no executions");
		}

		private bool CheckPathValidity(string path)
		{
			if(IsInvalidPath(path))
				throw new BadCommandException("directory "+ path);

			return true;
		}

		private bool IsInvalidPath(string path)
		{
			return path.IsNullOrWhitespace()
				|| path.IndexOfAny(Path.GetInvalidPathChars()) >= 0 
				|| path.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
		}

		[NotNull]
		public IExecution[] GetExecutions() => _executions ?? throw new InvalidOperationException();

		public void CreateExecutions()
		{
			_executions = Executions.Select((e, i) =>
			{
				try
				{
					return (IExecution)e.CreateExecution();
				}
				catch (BadCommandException ex)
				{
					throw new BadCommandException($"Execution {i} of Title {e.Title}:", ex);
				}
			}).ToArray();
		}
	}
}
