using NUnit.Framework;
using PicRefactoring.Actions;

namespace PicRefactoring.Tests.Actions
{
	[TestFixture]
	public class RenameRegexTests
	{
		[Test]
		public void GetRenamedFileName_CorrectResult()
		{
			var action = new RenameRegex(@"(?<=[\w]{3}).{1,15}", 9);
			action.CheckValidity();

			Assert.AreEqual("ename123", action.GetRenamedFileName("filename123"));
			Assert.AreEqual("ename1234", action.GetRenamedFileName("filename1234"));
			Assert.AreEqual("ename1234567890", action.GetRenamedFileName("filename1234567890-XDD"));
		}
	}
}
