﻿using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PicRefactoring.Commanding;

namespace PicRefactoring.Tests
{
	[TestFixture()]
	public class FileWrapperTests
	{
		[Test]
		public void GetWidth_ReturnsCorrectValue()
		{
			const int width = 1024;
			var fileWrapper = new FileWrapper(new FileInfo("nest_by_mrfloki-dcgzwvz.png"));

			Assert.AreEqual(width, fileWrapper.GetWidth());
		}

		[Test]
		public void GetExtension_ReturnsCorrect()
		{
			var fileWrapper = new FileWrapper(new FileInfo("nest_by_mrfloki-dcgzwvz.png"));

			Assert.AreEqual("png", fileWrapper.GetExtension());
		}
	}
}