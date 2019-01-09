using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using NSubstitute;
using NUnit.Framework;
using PicRefactoring.Actions;
using FileWrapper = PicRefactoring.Commanding.FileWrapper;

namespace PicRefactoring.Tests.Actions
{
	[TestFixture]
	public class RenameActionTests
	{
		private IFileSystem SetupFileSystem()
		{
			var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
			{
				{ @"c:\filename123.jpg", new MockFileData("Testing is meh.") },
				{ @"c:\textFile007.txt", new MockFileData("Testing is meh.") },
				{ @"c:\textFile.txt", new MockFileData("Testing is meh.") }
			});
			return fileSystem;
		}


		[Test]
		public void FileRename_NoFileNameCollision()
		{
			var rename = Substitute.For<Rename>();
			var action = new RenameAction(rename);
			var fileSystem = SetupFileSystem();
			var fileInfo = fileSystem.FileInfo.FromFileName("filename123.jpg");
			var file = new FileWrapper(fileInfo);

			rename.GetRenamedFileName("filename123").Returns("file_name");
			action.ActOnFile(file);

			Assert.AreEqual("file_name", file.GetFileName());
			Assert.AreEqual("jpg", file.GetExtension());
			Assert.True(fileSystem.File.Exists(@"c:\file_name.jpg"));
			Assert.False(fileSystem.File.Exists(@"c:\filename123.jpg"));
		}

		[Test]
		public void FileRename_FileNameCollision()
		{
			var rename = Substitute.For<Rename>();
			var action = new RenameAction(rename);
			var fileSystem = SetupFileSystem();
			var fileInfo   = fileSystem.FileInfo.FromFileName("textFile007.txt");
			var file       = new FileWrapper(fileInfo);

			rename.GetRenamedFileName("textFile007").Returns("textFile");
			rename.TryMakeFileNameUnique("textFile", "textFile007").Returns("textFile1");
			action.ActOnFile(file);

			Assert.AreEqual("textFile1", file.GetFileName());
			Assert.AreEqual("txt",       file.GetExtension());
			Assert.True(fileSystem.File.Exists(@"c:\textFile1.txt"));
			Assert.False(fileSystem.File.Exists(@"c:\textFile007.txt"));
		}
	}
}
