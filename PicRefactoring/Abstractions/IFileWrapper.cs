namespace PicRefactoring.Abstractions
{
	public interface IFileWrapper
	{
		int GetWeightInKb();

		int GetWidth();

		int GetHeight();

		string GetExtension();
	}
}
