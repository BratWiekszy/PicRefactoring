using System;
using PicRefactoring.Commanding;

namespace PicRefactoring.Actions
{
	public abstract class Rename
	{
		public const int MinimalLength = 4;

		protected Random _random;

		public int MinLength { get; protected set; }

		protected Rename()
		{
			_random = new Random();
		}

		public virtual void CheckValidity()
		{
			if(MinLength < MinimalLength)
				throw new BadCommandException();
		}

		public abstract string GetRenamedFileName(string fileName);

		public virtual string TryMakeFileNameUnique(string fileName, string originalFileName)
		{
			var randomChar = (char)('0' + _random.Next() % 10);
			return fileName + randomChar.ToString();
		}
	}
}
