using System.Linq;
using JsonRazor.Serialization;
using PicRefactoring.Actions;
using PicRefactoring.Predicates;

namespace PicRefactoring.Commanding
{
	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public class ExecutionEntry
	{
		public string           Title      { get; private set; }
		public PredicateEntry[] Predicates { get; private set; }
		public ActionEntry[]    Actions    { get; private set; }

		public ExecutionEntry() {}

		public ExecutionEntry(PredicateEntry[] predicates, ActionEntry[] actions)
		{
			Predicates = predicates;
			Actions = actions;
		}

		public Execution CreateExecution()
		{
			CheckValidity();

			var predicates = GetOrderedPredicates();
			var actions = GetActions();
			return new Execution(predicates, actions, this);
		}

		protected IFilePredicate[] GetOrderedPredicates() => 
			Predicates.Select(p => p.CreatePredicate()).OrderBy(p => p.Order).ToArray();

		protected IFileAction[] GetActions() => 
			Actions.Select(p => p.CreateAction(null)).ToArray();

		private void CheckValidity()
		{
			if(Predicates == null || Predicates.Length == 0)
				throw new BadCommandException();

			if(Actions == null || Actions.Length == 0)
				throw new BadCommandException();

			if(Predicates.Any(p => p == null))
				throw new BadCommandException();

			if(Actions.Any(a => a == null))
				throw new BadCommandException();
		}
	}
}
