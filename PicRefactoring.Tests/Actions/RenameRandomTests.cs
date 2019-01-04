using NUnit.Framework;
using PicRefactoring.Actions;

namespace PicRefactoring.Tests.Actions
{
	[TestFixture]
	public class RenameRandomTests
	{
		[Test]
		public void GetRenamedFileName_CorrectResult()
		{
			var action = new RenameRandom(9);
			action.CheckValidity();

			var name = action.GetRenamedFileName("filename123");
			Assert.NotNull(name);
			Assert.AreEqual(9, name.Length);

			var name2 = action.TryMakeFileNameUnique(name, "filename123");
			Assert.AreNotEqual(name, name2);
		}
	}
}
