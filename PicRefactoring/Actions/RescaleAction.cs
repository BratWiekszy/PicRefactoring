using JsonRazor.Serialization;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;

namespace PicRefactoring.Actions
{
	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public class RescaleAction : IFileAction
	{
		public const int MinSize = 32;

		public int MaxSize { get; private set; }

		public void CheckValidity()
		{
			if(MaxSize < MinSize)
				throw new BadCommandException();
		}

		public string ActOnFile(IFileWrapper file)
		{
			throw new System.NotImplementedException();
		}
	}
}
