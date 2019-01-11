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

		public Commands() {}

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

			if(Executions.Any(e => e == null))
				throw new BadCommandException("execution null");
		}

		private bool CheckPathValidity(string path)
		{
			if(IsInvalidPath(path))
				throw new BadCommandException("directory "+ path);

			return true;
		}

		private bool IsInvalidPath(string path)
		{
			var inv1 = Path.GetInvalidPathChars();
			if (path.IsNullOrWhitespace())
				return true;

			return path.IndexOfAny(inv1) >= 0;
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
