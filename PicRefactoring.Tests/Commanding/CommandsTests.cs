using NUnit.Framework;
using PicRefactoring.Commanding;

namespace PicRefactoring.Tests.Commanding
{
	[TestFixture]
	public class CommandsTests
	{
		[Test]
		public void CheckValidity_ThrowsOnInvalidDirPaths()
		{
			Assert.Throws<BadCommandException>(() => new Commands(new[] { @"??><>.-&^%$#@!" }, null).CheckValidity());
			Assert.Throws<BadCommandException>(() 
				=> new Commands(new[] {@"c:\txt.txt", @"??><>\\txt.png" }, null).CheckValidity());

			Assert.Throws<BadCommandException>(() => new Commands(new[] { @"d:\elo.jpg" }, null).CheckValidity());
		}

		[Test]
		public void CheckValidity_NotThrowOnValidDirPaths()
		{
			Assert.DoesNotThrow(() 
				=> new Commands(new[] { @"c:\dir\fil 2" }, new []{ new ExecutionEntry(), }).CheckValidity()
			);
			Assert.DoesNotThrow(() 
				=> new Commands(new[] {@"c:\dir__-00f 3" }, new []{ new ExecutionEntry(), }).CheckValidity()
			);
		}
	}
}
