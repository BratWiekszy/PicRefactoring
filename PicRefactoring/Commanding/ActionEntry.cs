using System;
using JsonRazor;
using JsonRazor.Serialization;

namespace PicRefactoring.Commanding
{
	public enum ActionType
	{
		Rescale,
		Rename,
		SuggestDuplicates
	}

	[JsonInfo(new Type[]{typeof(string), typeof(object)}, true, 
			  nameof(ActionEntry.Type), nameof(ActionEntry.Value), Members = ModelMembers.None)]
	public class ActionEntry
	{
		private ActionType _type;

		public string Type { get; }
		public object Value { get; }

		public ActionEntry(string Type, object Value)
		{
			this.Type = Type;
			this.Value = Value;

			CheckGeneralValiditity();
			ConvertType();
		}

		private void CheckGeneralValiditity()
		{
			if(Type.IsNullOrWhitespace())
				throw new BadCommandException();

			if(Value == null || !(Value is JsonToken))
				throw new BadCommandException();
		}

		private void ConvertType()
		{
			if(Enum.TryParse<ActionType>(Type, true, out _type) == false)
				throw new BadCommandException();
		}
	}
}
