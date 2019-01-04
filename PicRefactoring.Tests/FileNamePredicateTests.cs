using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;
using PicRefactoring.Predicates;

namespace PicRefactoring.Tests
{
	[TestFixture()]
	public class FileNamePredicateTests
	{
		[Test]
		public void BadRegexesAreDetected()
		{
			Assert.Throws<BadCommandException>(() => new FileNamePredicate(new[] { "", null, "	" }));
		}
		
		[TestCase("filename1234567")]
		[TestCase("0afilename1234567890")]
		[TestCase(@"__arima_senne_original_drawn_by_kashiwamochi_yomogi__sample-254b3de30c3a8abf49c1adfa1a0d8493")]
		public void FileMatches_YieldsPositive(string fileName)
		{
			var predicate = new FileNamePredicate(
				new[] { @"^[\w\d]{15,}$", @"^[\d][\w]{20,}$", @"^__([\w_\d]+)[_-]{1,2}sample[-_]{1,2}[\dabcdef]+$" }
			);
			var file      = Substitute.For<IFileWrapper>();
			file.GetFileName().Returns(fileName);

			Assert.True(predicate.FileMatches(file));
		}

		[TestCase("filename1")]
		[TestCase("0afile_-name_1234567890")]
		[TestCase(@"__arima_senne_original_drawn_by%%_kashiwamochi&_yomogi")]
		public void FileMatches_YieldsNegative(string fileName)
		{
			var predicate = new FileNamePredicate(
				new[] { @"^[\w\d]{15,}$", @"^[\d][\w]{20,}$", @"^__([\w_\d]+)[_-]{1,2}sample[-_]{1,2}[\dabcdef]+$" }
			);
			var file = Substitute.For<IFileWrapper>();
			file.GetFileName().Returns(fileName);

			Assert.False(predicate.FileMatches(file));
		}
	}
}
