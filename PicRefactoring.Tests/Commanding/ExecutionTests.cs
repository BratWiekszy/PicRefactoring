using NSubstitute;
using NUnit.Framework;
using PicRefactoring.Abstractions;
using PicRefactoring.Actions;
using PicRefactoring.Commanding;
using PicRefactoring.Predicates;

namespace PicRefactoring.Tests.Commanding
{
	[TestFixture]
	public class ExecutionTests
	{
		[Test]
		public void ExecuteAction_NoActionExecutedIfPredicateFalse()
		{
			var file = Substitute.For<IFileWrapper>();
			var predicate = Substitute.For<IFilePredicate>();
			var action    = Substitute.For<IFileAction>();

			file.GetExtension().Returns("txt");
			file.GetFileName().Returns("fn");
			predicate.FileMatches(file).Returns(false);

			var execution = new Execution(new[] { predicate }, new[] { action });
			execution.ExecuteAction(file);

			action.DidNotReceive().ActOnFile(file);
		}

		[Test]
		public void ExecuteAction_ActionExecutedIfPredicateTrue()
		{
			var file      = Substitute.For<IFileWrapper>();
			var predicate = Substitute.For<IFilePredicate>();
			var action    = Substitute.For<IFileAction>();

			file.GetExtension().Returns("txt");
			file.GetFileName().Returns("fn");
			predicate.FileMatches(file).Returns(true);

			var execution = new Execution(new[] { predicate }, new[] { action });
			execution.ExecuteAction(file);

			action.Received(1).ActOnFile(file);
		}

		[Test]
		public void ExecuteAction_PredicateConditionsMergedWithAnd()
		{
			var file      = Substitute.For<IFileWrapper>();
			var predicate1 = Substitute.For<IFilePredicate>();
			var predicate2 = Substitute.For<IFilePredicate>();
			var action    = Substitute.For<IFileAction>();

			file.GetExtension().Returns("txt");
			file.GetFileName().Returns("fn");
			predicate1.FileMatches(file).Returns(true);
			predicate2.FileMatches(file).Returns(false);

			var execution = new Execution(new[] { predicate1, predicate2 }, new[] { action });
			execution.ExecuteAction(file);

			action.DidNotReceive().ActOnFile(file);
		}
	}
}
