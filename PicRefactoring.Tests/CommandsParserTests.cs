using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;

namespace PicRefactoring.Tests
{
	[TestFixture]
	public class CommandsParserTests
	{
		[Test]
		public void OpenCommandsFile_ThrowsWhenCommandsDontExist()
		{
			var fileSys = new MockFileSystem();
			var parser = new CommandsParser(fileSys);

			Assert.Throws<FileNotFoundException>(() => parser.OpenCommandsFile(null));
			Assert.Throws<FileNotFoundException>(() => parser.OpenCommandsFile(new string[0]));
			Assert.Throws<FileNotFoundException>(() => parser.OpenCommandsFile(new []{ @"x:\non_existent+path\"}));
			Assert.Throws<FileNotFoundException>(() 
				=> parser.OpenCommandsFile(new []{ @"x:\non_existent_path\commands.jsom"}));
		}

		[Test]
		public void OpenCommandsFile_FileFoundViaArgsExplicit()
		{
			var fileSys = new MockFileSystem(new Dictionary<string, MockFileData>()
			{
				{@"x:\path\cmd.json", new MockFileData("{}")}
			});
			var parser  = new CommandsParser(fileSys);

			Assert.DoesNotThrow(() => parser.OpenCommandsFile(new []{@"x:\path\cmd.json"}));
			Assert.NotNull(parser.CommandFile);
			Assert.AreEqual("cmd.json", parser.CommandFile.Name);
		}

		[Test]
		public void OpenCommandsFile_FileFoundViaArgsImplicit()
		{
			var fileSys = new MockFileSystem(new Dictionary<string, MockFileData>()
			{
				{@"x:\path\cmd.json", new MockFileData("{}")},
				{@"x:\path\commands.json", new MockFileData("{huehue}")}
			});
			var parser = new CommandsParser(fileSys);

			Assert.DoesNotThrow(() => parser.OpenCommandsFile(new []{@"x:\path"}));
			Assert.NotNull(parser.CommandFile);
			Assert.AreEqual("commands.json", parser.CommandFile.Name);
		}
	}
}
