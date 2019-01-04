﻿using NUnit.Framework;
using PicRefactoring.Commanding;

namespace PicRefactoring.Tests
{
	[TestFixture()]
	public class PredicateEntryTests
	{
		[Test]
		public void DetectsBadDeclaration()
		{
			Assert.Throws<BadCommandException>(() => new PredicateEntry(null, null, null));
			Assert.Throws<BadCommandException>(() => new PredicateEntry(null, new []{"str"}, null));

			Assert.Throws<BadCommandException>(() => new PredicateEntry("properties", null, null));
			Assert.Throws<BadCommandException>(() => new PredicateEntry("properties", new []{"str"}, null));
			Assert.Throws<BadCommandException>(() => 
				new PredicateEntry("properties", new []{"str"}, new PropertiesDetails())
			);

			Assert.Throws<BadCommandException>(() => new PredicateEntry("regex", null, null));
			Assert.Throws<BadCommandException>(() => new PredicateEntry("regex", null, new PropertiesDetails()));

			Assert.Throws<BadCommandException>(() => new PredicateEntry("extension", null, null));
			Assert.Throws<BadCommandException>(() => new PredicateEntry("extension", null, new PropertiesDetails()));
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
			Assert.Throws<BadCommandException>(() => new PredicateEntry("uhh", new []{"str"}, null));
			Assert.Throws<BadCommandException>(() => new PredicateEntry("Elo", new []{"str"}, null));
			Assert.Throws<BadCommandException>(() => new PredicateEntry("REGEXP", new []{"str"}, null));
			Assert.Throws<BadCommandException>(() => new PredicateEntry("extention", new []{"str"}, null));
		}
	}
}
