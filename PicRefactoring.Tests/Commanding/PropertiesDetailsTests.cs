using NSubstitute;
using NUnit.Framework;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;

namespace PicRefactoring.Tests.Commanding
{
	[TestFixture]
	public class PropertiesDetailsTests
	{
		[Test]
		public void Prepare_ChecksValidData()
		{
			var noPropertySet = new PropertiesDetails();
			Assert.Throws<BadCommandException>(noPropertySet.Prepare);

			var noComparisonSet = new PropertiesDetails(null, 1000, 1000);
			Assert.Throws<BadCommandException>(noComparisonSet.Prepare);

			var badComparisonSet = new PropertiesDetails("Xor", 1000, 1000);
			Assert.Throws<BadCommandException>(badComparisonSet.Prepare);
		}

		[TestCase("AND", Description = "upper case and")]
		[TestCase("OR", Description = "upper case")]
		[TestCase("Or", Description = "capitalized")]
		[TestCase("or", Description = "lower case")]
		public void Prepare_ComparisonAintTooStrict(string comparison)
		{
			var comparisonSet = new PropertiesDetails(comparison, 100, 100);
			Assert.DoesNotThrow(comparisonSet.Prepare);
		}

		[TestCase("or", 1000, 1200, 800, 2800, 900)]
		[TestCase("or", 1000, 1200, 2000, 700, 900)]
		[TestCase("and", 1000, 1200, 1000, 2800, 900)]
		[TestCase("and", 1000, 1200, 4000, 800, 1900)]
		public void FileMatches_YieldsPositive(string comp, int weight, int size, 
											   int fileWeight, int fileWidth, int fileHeight)
		{
			var details = new PropertiesDetails(comp, weight, size);
			details.Prepare();

			var file = Substitute.For<IFileWrapper>();
			file.GetWidth().Returns(fileWidth);
			file.GetHeight().Returns(fileHeight);
			file.GetWeightInKb().Returns(fileWeight);

			Assert.True(details.FileMatches(file));
		}

		[TestCase("or",  1000, 1200, 800,  900, 900)]
		[TestCase("or",  1000, 1200, 200, 700,  900)]
		[TestCase("and", 1000, 1200, 900, 2800, 900)]
		[TestCase("and", 1000, 1200, 4000, 800,  1100)]
		public void FileMatches_YieldsNegative(string comp, int weight, int size, 
											   int fileWeight, int fileWidth, int fileHeight)
		{
			var details = new PropertiesDetails(comp, weight, size);
			details.Prepare();

			var file = Substitute.For<IFileWrapper>();
			file.GetWidth().Returns(fileWidth);
			file.GetHeight().Returns(fileHeight);
			file.GetWeightInKb().Returns(fileWeight);

			Assert.False(details.FileMatches(file));
		}
	}
}