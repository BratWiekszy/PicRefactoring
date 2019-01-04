using NUnit.Framework;
using PicRefactoring.Actions;

namespace PicRefactoring.Tests.Actions
{
	[TestFixture]
	public class RenameCutTests
	{
		[Test]
		public void GetRenamedFileName_CorrectResult()
		{
			var action = new RenameCut(9);
			action.CheckValidity();

			Assert.AreEqual("file_name", action.GetRenamedFileName("file_name123456789"));
			Assert.AreEqual("file_name", action.GetRenamedFileName("file_name"));
			Assert.AreEqual("filename", action.GetRenamedFileName("filename"));

			var fn = action.GetRenamedFileName("file_name_&_123456789");
			fn = action.TryMakeFileNameUnique(fn, "file_name_&_123456789");
			fn = action.TryMakeFileNameUnique(fn, "file_name_&_123456789");
			fn = action.TryMakeFileNameUnique(fn, "file_name_&_123456789");
			Assert.AreEqual("file_name_&_", fn);
		}
	}
}
