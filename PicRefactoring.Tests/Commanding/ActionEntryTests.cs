using JsonRazor;
using NUnit.Framework;
using PicRefactoring.Commanding;

namespace PicRefactoring.Tests.Commanding
{
	[TestFixture]
	public class ActionEntryTests
	{
		[Test]
		public void DetectsBadDeclaration()
		{
			Assert.Throws<BadCommandException>(() => new ActionEntry(null, null));
			Assert.Throws<BadCommandException>(() => new ActionEntry("", null));
			Assert.Throws<BadCommandException>(() => new ActionEntry("	", null));

			Assert.Throws<BadCommandException>(() => new ActionEntry("rescale", null));
			Assert.Throws<BadCommandException>(() => new ActionEntry("rename", null));
			Assert.Throws<BadCommandException>(() => new ActionEntry("suggestDuplicates", null));

			Assert.Throws<BadCommandException>(() => new ActionEntry("rescale", new object()));
			Assert.Throws<BadCommandException>(() => new ActionEntry("rename", new object()));
			Assert.Throws<BadCommandException>(() => new ActionEntry("suggestDuplicates", new object()));
		}

		[Test]
		public void DetectsNotSupportedActionTypes()
		{
			Assert.Throws<BadCommandException>(() => new ActionEntry("scale", new JsonToken()));
			Assert.Throws<BadCommandException>(() => new ActionEntry("Name", new JsonToken()));
			Assert.Throws<BadCommandException>(() => new ActionEntry("duplicates", new JsonToken()));
		}

		[Test]
		public void ActionTypesDeclarationAintTooStrict()
		{
			Assert.DoesNotThrow(() => new ActionEntry("Rescale",      new JsonToken()));
			Assert.DoesNotThrow(() => new ActionEntry("rename",       new JsonToken()));
			Assert.DoesNotThrow(() => new ActionEntry("SUGGESTDUPLICATES", new JsonToken()));
		}
	}
}
