using System.Text.RegularExpressions;
using JsonRazor.Serialization;
using PicRefactoring.Commanding;

namespace PicRefactoring.Actions
{
	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public sealed class RenameRegex : Rename
	{
		private Regex _regex;

		public string OutputRegex { get; private set; }

		public RenameRegex() {}

		public RenameRegex(string outputRegex, int minLength) 
		{
			OutputRegex = outputRegex;
			MinLength = minLength;
		}

		public override void CheckValidity()
		{
			base.CheckValidity();
			if(OutputRegex.IsNullOrWhitespace())
				throw new BadCommandException();

			_regex = new Regex(OutputRegex);
		}

		public override string GetRenamedFileName(string fileName)
		{
			var match = _regex.Match(fileName);
			return match.Value;
		}
	}
}
