using NSubstitute;
using NUnit.Framework;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;
using PicRefactoring.Predicates;

namespace PicRefactoring.Tests.Predicates
{
	[TestFixture]
	public class FilePropertiesPredicateTests
	{
		[TestCase("or",  1000, 1200, 800,  2800, 900)]
		[TestCase("or",  1000, 1200, 2000, 700,  900)]
		[TestCase("and", 1000, 1200, 1000, 2800, 900)]
		[TestCase("and", 1000, 1200, 4000, 800,  1900)]
		public void FileMatches_YieldsPositive(string comp, int weight, int size, 
											   int fileWeight, int fileWidth, int fileHeight)
		{
			var details = new PropertiesDetails(comp, weight, size);
			var predicate = new FilePropertiesPredicate(details);

			var file = Substitute.For<IFileWrapper>();
			file.GetWidth().Returns(fileWidth);
			file.GetHeight().Returns(fileHeight);
			file.GetWeightInKb().Returns(fileWeight);

			Assert.True(predicate.FileMatches(file));
		}

		[TestCase("or",  1000, 1200, 800,  900,  900)]
		[TestCase("or",  1000, 1200, 200,  700,  900)]
		[TestCase("and", 1000, 1200, 900,  2800, 900)]
		[TestCase("and", 1000, 1200, 4000, 800,  1100)]
		public void FileMatches_YieldsNegative(string comp, int weight, int size, 
											   int fileWeight, int fileWidth, int fileHeight)
		{
			var details = new PropertiesDetails(comp, weight, size);
			var predicate = new FilePropertiesPredicate(details);

			var file = Substitute.For<IFileWrapper>();
			file.GetWidth().Returns(fileWidth);
			file.GetHeight().Returns(fileHeight);
			file.GetWeightInKb().Returns(fileWeight);

			Assert.False(predicate.FileMatches(file));
		}
	}
}
