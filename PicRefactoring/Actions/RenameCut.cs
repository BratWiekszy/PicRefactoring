using JsonRazor.Serialization;

namespace PicRefactoring.Actions
{
	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public sealed class RenameCut : Rename
	{
		public RenameCut() {}

		public RenameCut(int minLength)
		{
			MinLength = minLength;
		}

		public override string TryMakeFileNameUnique(string fileName, string originalFileName)
		{
			var nameLength = fileName.Length;
			if (nameLength < originalFileName.Length)
			{
				char nextSrcChar = originalFileName[nameLength];
				return fileName + nextSrcChar.ToString();
			}
			return base.TryMakeFileNameUnique(fileName, originalFileName);
		}

		public override string GetRenamedFileName(string fileName)
		{
			if (fileName.Length <= MinLength)
				return fileName;

			return fileName.Substring(0, MinLength);
		}
	}
}
