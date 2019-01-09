using System;
using JetBrains.Annotations;
using JsonRazor;
using JsonRazor.Serialization;
using PicRefactoring.Abstractions;
using PicRefactoring.Actions;

namespace PicRefactoring.Commanding
{
	public enum ActionType
	{
		Rescale,
		RenameCut,
		RenameRandom,
		RenameRegex,
		DetectDuplicates
	}

	[JsonInfo(new Type[]{typeof(string), typeof(object)}, true, 
			  nameof(ActionEntry.Type), nameof(ActionEntry.Value), Members = ModelMembers.None)]
	public class ActionEntry
	{
		private ActionType _type;

		public string Type { get; }
		public object Value { get; }

		public ActionEntry() {}

		public ActionEntry(string Type, object Value)
		{
			this.Type = Type;
			this.Value = Value;

			CheckGeneralValiditity();
			ConvertType();
			CheckValidityPerType();
		}

		private void CheckGeneralValiditity()
		{
			if(Type.IsNullOrWhitespace())
				throw new BadCommandException();

			if(Value != null && !(Value is JsonToken))
				throw new BadCommandException();
		}

		private void ConvertType()
		{
			if(Enum.TryParse<ActionType>(Type, true, out _type) == false)
				throw new BadCommandException();
		}

		private void CheckValidityPerType()
		{
			switch (_type)
			{
				case ActionType.RenameCut:
				case ActionType.RenameRandom:
				case ActionType.RenameRegex:
				case ActionType.Rescale: {
					if(Value == null) throw new BadCommandException();
				}
					break;
				case ActionType.DetectDuplicates: {
					if(Value != null) throw new BadCommandException();
				}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		[NotNull]
		public virtual IFileAction CreateAction([NotNull] IDependencyCreator creator)
		{
			var token = Value as JsonObject;
			switch (_type)
			{
				case ActionType.Rescale:
					return token.ConvertSelf<RescaleAction>();
				case ActionType.RenameCut: {
					return CreateRenameAction<RenameCut>(token);
				}
				case ActionType.RenameRandom: {
					return CreateRenameAction<RenameRandom>(token);
				}
				case ActionType.RenameRegex: {
					return CreateRenameAction<RenameRegex>(token);
				}
				case ActionType.DetectDuplicates:
					return new DetectDuplicatesAction();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private RenameAction CreateRenameAction<T>(JsonObject token) where T : Rename
		{
			var rename = token.ConvertSelf<T>();
			return new RenameAction(rename);
		}
	}
}
