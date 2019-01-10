using System;
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
			var actions    = GetActions();
			return new Execution(predicates, actions, this);
		}

		protected IFilePredicate[] GetOrderedPredicates() => 
			Predicates.Select((p, i) =>
			{
				try
				{
					p.CheckValidity();
					return p.CreatePredicate();
				}
				catch (BadCommandException e)
				{
					throw new BadCommandException($"Predicate {i}:", e);
				}
			}).OrderBy(p => p.Order).ToArray();

		protected IFileAction[] GetActions() => 
			Actions.Select((a, i) =>
			{
				try
				{
					a.CheckValidity();
					return a.CreateAction(null);
				}
				catch (BadCommandException e)
				{
					throw new BadCommandException($"Action {i}:", e);
				}
			}).ToArray();

		private void CheckValidity()
		{
			if(Predicates == null || Predicates.Length == 0)
				throw new BadCommandException("no predicates");

			if(Actions == null || Actions.Length == 0)
				throw new BadCommandException("no actions");

			CheckNullInArray(Predicates, "predicate");
			CheckNullInArray(Actions, "action");
		}

		private void CheckNullInArray(Array array, string type)
		{
			int i = 0;
			if((i = Array.IndexOf(array, null)) >= 0)
				throw new BadCommandException($"{type} null at {i}");
		}
	}
}
