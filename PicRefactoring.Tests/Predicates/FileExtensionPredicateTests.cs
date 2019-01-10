using NSubstitute;
using NUnit.Framework;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;
using PicRefactoring.Predicates;

namespace PicRefactoring.Tests.Predicates
{
	[TestFixture()]
	public class FileExtensionPredicateTests
	{
		[TestCase("jpg")]
		[TestCase("png")]
		[TestCase("jpeg")]
		public void FileMatches_YieldsPositive(string ext)
		{
			var predicate = new FileExtensionPredicate(new[] { "jpg", "png", "jpeg" });
			var file = Substitute.For<IFileWrapper>();
			file.GetExtension().Returns(ext);

			Assert.True(predicate.FileMatches(file));
		}

		[TestCase("mp4")]
		[TestCase("p1ng")]
		[TestCase("tiff")]
		public void FileMatches_YieldsNegative(string ext)
		{
			var predicate = new FileExtensionPredicate(new[] { "jpg", "png", "jpeg" });
			var file      = Substitute.For<IFileWrapper>();
			file.GetExtension().Returns(ext);

			Assert.False(predicate.FileMatches(file));
		}

		[TestCase("jpg")]
		[TestCase("png")]
		[TestCase("jpeg")]
		public void FileMatches_MatchingAintTooStrict(string ext)
		{
			var predicate = new FileExtensionPredicate(new[] { "PNG", ".jpg", ".JPEG" });
			predicate.Prepare();
			var file      = Substitute.For<IFileWrapper>();
			file.GetExtension().Returns(ext);

			Assert.True(predicate.FileMatches(file));
		}

		[Test]
		public void DetectsBadExtensions()
		{
			Assert.Throws<BadCommandException>(() => new FileExtensionPredicate(new[] { "", null, "	" }).Prepare());
		}
	}
}
