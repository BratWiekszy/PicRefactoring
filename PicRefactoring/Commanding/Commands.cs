using System.IO;
using System.Linq;
using JetBrains.Annotations;
using JsonRazor.Serialization;

namespace PicRefactoring.Commanding
{
	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public class Commands
	{
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
				throw new BadCommandException();

			if(Directories.Any(d => IsInvalidPath(d)))
				throw new BadCommandException();

			if(Executions == null || Executions.Length == 0)
				throw new BadCommandException();
		}

		private bool IsInvalidPath(string path)
		{
			return path.IsNullOrWhitespace()
				|| path.IndexOfAny(Path.GetInvalidPathChars()) >= 0 
				|| path.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
		}

		[NotNull]
		public Execution[] CreateExecutions()
		{
			return Executions.Select(e => e.CreateExecution()).ToArray();
		}
	}
}
