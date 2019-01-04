using System.IO;
using JsonRazor.Serialization;

namespace PicRefactoring.Actions
{
	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public sealed class RenameRandom : Rename
	{
		public RenameRandom() {}

		public RenameRandom(int minLength)
		{
			MinLength = minLength;
		}

		public override string GetRenamedFileName(string fileName)
		{
			var name = Path.GetRandomFileName();
			if (name.Length <= MinLength)
				return name;

			return name.Substring(0, MinLength);
		}
	}
}
