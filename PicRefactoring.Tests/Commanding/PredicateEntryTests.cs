using NUnit.Framework;
using PicRefactoring.Commanding;

namespace PicRefactoring.Tests.Commanding
{
	[TestFixture()]
	public class PredicateEntryTests
	{
		[Test]
		public void DetectsBadDeclaration()
		{
			Assert.Throws<BadCommandException>(() => new PredicateEntry(null, null, null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new PredicateEntry(null, new []{"str"}, null).CheckValidity());

			Assert.Throws<BadCommandException>(() => new PredicateEntry("properties", null, null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new PredicateEntry("properties", new []{"str"}, null).CheckValidity());
			Assert.Throws<BadCommandException>(() => 
				new PredicateEntry("properties", new []{"str"}, new PropertiesDetails()).CheckValidity()
			);

			Assert.Throws<BadCommandException>(() => new PredicateEntry("regex", null, null).CheckValidity());
			Assert.Throws<BadCommandException>(() 
				=> new PredicateEntry("regex", null, new PropertiesDetails()).CheckValidity()
			);

			Assert.Throws<BadCommandException>(() => new PredicateEntry("extension", null, null).CheckValidity());
			Assert.Throws<BadCommandException>(() 
				=> new PredicateEntry("extension", null, new PropertiesDetails()).CheckValidity()
			);
		}

		[Test]
		public void PredicateTypeAintTooStrict()
		{
			Assert.DoesNotThrow(() => new PredicateEntry("regex", new []{"str"}, null));
			Assert.DoesNotThrow(() => new PredicateEntry("Regex", new []{"str"}, null));
			Assert.DoesNotThrow(() => new PredicateEntry("REGEX", new []{"str"}, null));

			Assert.DoesNotThrow(() => new PredicateEntry("EXTENSION", new []{"str"}, null));
			Assert.DoesNotThrow(() => new PredicateEntry("Extension", new []{"str"}, null));
			Assert.DoesNotThrow(() => new PredicateEntry("extension", new []{"str"}, null));
		}

		[Test]
		public void DetectsNotSupportedPredicateType()
		{
			Assert.Throws<BadCommandException>(() => new PredicateEntry("uhh", new []{"str"}, null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new PredicateEntry("Elo", new []{"str"}, null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new PredicateEntry("REGEXP", new []{"str"}, null).CheckValidity());
			Assert.Throws<BadCommandException>(() => new PredicateEntry("extention", new []{"str"}, null).CheckValidity());
		}
	}
}
