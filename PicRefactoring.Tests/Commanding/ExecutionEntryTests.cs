using NSubstitute;
using NUnit.Framework;
using PicRefactoring.Commanding;
using PicRefactoring.Predicates;

namespace PicRefactoring.Tests.Commanding
{
	[TestFixture]
	public class ExecutionEntryTests 
	{

		class Test : ExecutionEntry
		{
			public Test(PredicateEntry[] predicates, ActionEntry[] actions) : base(predicates, actions) {}

			public IFilePredicate[] RunGetOrderedPredicates()
			{
				return base.GetOrderedPredicates();
			} 
		}

		[Test]
		public void CreateExecution_ThrowsIfNoPredicateOrAction()
		{
			Assert.Throws<BadCommandException>(() => new ExecutionEntry().CreateExecution());
			Assert.Throws<BadCommandException>(() => 
					new ExecutionEntry(new PredicateEntry[0], new ActionEntry[0]).CreateExecution());
			Assert.Throws<BadCommandException>(() => 
					new ExecutionEntry(new PredicateEntry[0], null).CreateExecution());
		}

		[Test]
		public void CreateExecution_EnsureCorrectOrder()
		{
			var pred1 = Substitute.For<PredicateEntry>();
			var pred2 = Substitute.For<PredicateEntry>();
			var fp1 = Substitute.For<IFilePredicate>();
			var fp2 = Substitute.For<IFilePredicate>();
			pred1.CreatePredicate().Returns(fp1);
			pred2.CreatePredicate().Returns(fp2);
			fp1.Order.Returns(2);
			fp2.Order.Returns(1);

			var entry = new Test(new []{pred1,pred2}, new []{Substitute.For<ActionEntry>()});

			var correctOrder = new IFilePredicate[] { fp2, fp1 };
			CollectionAssert.AreEqual(correctOrder, entry.RunGetOrderedPredicates());
		}
	}
}
