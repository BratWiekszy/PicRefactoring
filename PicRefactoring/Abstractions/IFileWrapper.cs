using System.IO.Abstractions;

namespace PicRefactoring.Abstractions
{
	public interface IFileWrapper
	{
		int GetWeightInKb();

		int GetWidth();

		int GetHeight();

		string GetExtension();

		string GetFileName();

		string GetFileNameWithExtension();

		FileInfoBase UnderlyingFile { get; }
	}
}
