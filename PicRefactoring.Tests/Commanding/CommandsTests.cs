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
	}
}
