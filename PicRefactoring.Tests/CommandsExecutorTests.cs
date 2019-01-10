using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using NSubstitute;
using NUnit.Framework;
using PicRefactoring.Abstractions;

namespace PicRefactoring.Tests
{
	[TestFixture]
	public class CommandsExecutorTests
	{
		[Test]
		public void ExecuteCommands_AllDirectoriesProcessed()
		{
			var fs = new MockFileSystem(new Dictionary<string, MockFileData>()
				{
					{ @"c:\dir1\file1.txt", new MockFileData("data") },
					{ @"c:\dir1\_file2.txt", new MockFileData("data") },
					{ @"c:\dir2\file1.txt", new MockFileData("data") },
					{ @"c:\dir2\_file2.txt", new MockFileData("data") },
				}
			);
			var exec = Substitute.For<IExecution>();
			exec.Title.Returns("exec");
			exec.ExecuteAction(null).ReturnsForAnyArgs(c =>
				{
					var f = c.ArgAt<IFileWrapper>(0).UnderlyingFile;
					if (f.Name.StartsWith("_"))
					{
						f.MoveTo(Path.Combine(f.Directory.FullName, f.Name.Substring(1)));
						return true;
					}
					return false;
				}
			);
			var cmd = Substitute.For<ICommands>();
			cmd.Directories.Returns(new[] { @"c:\dir1\", @"c:\dir2" });
			cmd.GetExecutions().Returns(new []{ exec });

			var executor = new CommandsExecutor(fs, cmd);
			executor.ExecuteCommands();

			Assert.True(fs.FileExists(@"c:\dir1\file1.txt"));
			Assert.True(fs.FileExists(@"c:\dir2\file1.txt"));

			Assert.True(fs.FileExists(@"c:\dir1\file2.txt"));
			Assert.True(fs.FileExists(@"c:\dir2\file2.txt"));
		}

		[Test]
		public void ExecuteCommands_OnlyFilesProcessed()
		{
			var fs = new MockFileSystem(new Dictionary<string, MockFileData>()
				{
					{ @"c:\dir1\file1.txt", new MockFileData("data") },
					{ @"c:\dir1\_file2.txt", new MockFileData("data") },
					{ @"c:\dir2\file1.txt", new MockFileData("data") },
					{ @"c:\dir2\_file2.txt", new MockFileData("data") },
					{ @"c:\dir1\files", new MockDirectoryData() },
					{ @"c:\dir2\_files", new MockDirectoryData() },
				}
			);
			var exec = Substitute.For<IExecution>();
			exec.Title.Returns("exec");
			exec.ExecuteAction(null).ReturnsForAnyArgs(c =>
				{
					var f = c.ArgAt<IFileWrapper>(0).UnderlyingFile;
					if (f.Name.StartsWith("_"))
					{
						f.MoveTo(Path.Combine(f.Directory.FullName, f.Name.Substring(1)));
						return true;
					}
					return false;
				}
			);
			var cmd = Substitute.For<ICommands>();
			cmd.Directories.Returns(new[] { @"c:\dir1\", @"c:\dir2" });
			cmd.GetExecutions().Returns(new []{ exec });

			var executor = new CommandsExecutor(fs, cmd);
			executor.ExecuteCommands();

			Assert.True(fs.FileExists(@"c:\dir1\file1.txt"));
			Assert.True(fs.FileExists(@"c:\dir2\file1.txt"));

			Assert.True(fs.FileExists(@"c:\dir1\file2.txt"));
			Assert.True(fs.FileExists(@"c:\dir2\file2.txt"));

			Assert.True(fs.Directory.Exists(@"c:\dir1\files"));
			Assert.True(fs.Directory.Exists(@"c:\dir2\_files"));
		}
	}
}
