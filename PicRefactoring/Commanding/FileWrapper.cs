using System;
using System.Drawing;
using System.IO.Abstractions;
using JetBrains.Annotations;
using PicRefactoring.Abstractions;

namespace PicRefactoring.Commanding
{
	public class FileWrapper : IFileWrapper
	{
		private FileInfoBase _file;

		public FileWrapper([NotNull] FileInfoBase file)
		{
			_file = file;
		}

		public int GetWeightInKb()
		{
			return Math.Max(4, (int)(_file.Length / 1024));
		}

		public int GetWidth()
		{
			using (Image bitmap = Image.FromFile(_file.FullName))
			{
				return bitmap.Width;
			}
		}

		public int GetHeight()
		{
			using (Image bitmap = Image.FromFile(_file.FullName))
			{
				return bitmap.Height;
			}
		}

		public string GetExtension()
		{
			var ext = _file.Extension;
			if (ext.StartsWith('.'))
				ext = ext.Substring(1);
			return ext;
		}
	}
}
