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
			Assert.Throws<BadCommandException>(() => new ActionEntry(null, null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new ActionEntry("", null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new ActionEntry("	", null).CheckValidity());

			Assert.Throws<BadCommandException>(() => new ActionEntry("rescale", null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new ActionEntry("rename", null).CheckValidity());

			Assert.Throws<BadCommandException>(() => new ActionEntry("rescale", new object()).CheckValidity());
			Assert.Throws<BadCommandException>(() => new ActionEntry("rename", new object()).CheckValidity());
			Assert.Throws<BadCommandException>(() => new ActionEntry("suggestDuplicates", new object()).CheckValidity());
		}

		[Test]
		public void DetectsNotSupportedActionTypes()
		{
			Assert.Throws<BadCommandException>(() => new ActionEntry("scale", new JsonToken()).CheckValidity());
			Assert.Throws<BadCommandException>(() => new ActionEntry("Name", new JsonToken()).CheckValidity());
			Assert.Throws<BadCommandException>(() => new ActionEntry("duplicates", new JsonToken()).CheckValidity());
		}

		[Test]
		public void ActionTypesDeclarationAintTooStrict()
		{
			Assert.DoesNotThrow(() => new ActionEntry("Rescale", new JsonToken()).CheckValidity());
			Assert.DoesNotThrow(() => new ActionEntry("renameCut", new JsonToken()).CheckValidity());
			Assert.DoesNotThrow(() => new ActionEntry("renamerandom", new JsonToken()).CheckValidity());
			Assert.DoesNotThrow(() => new ActionEntry("RenameRegex", new JsonToken()).CheckValidity());
			Assert.DoesNotThrow(() => new ActionEntry("DETECTDUPLICATES", null).CheckValidity());
		}
	}
}
