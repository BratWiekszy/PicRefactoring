using System;
using JetBrains.Annotations;
using JsonRazor.Serialization;
using PicRefactoring.Predicates;

namespace PicRefactoring.Commanding
{
	public enum PredicateType
	{
		Extension,
		Regex,
		Properties
	}

	[JsonInfo(new Type[]{typeof(string), typeof(string[]), typeof(PropertiesDetails)}, true, 
			  nameof(PredicateEntry.Type), nameof(PredicateEntry.Value), nameof(PredicateEntry.Details),
			  Members = ModelMembers.None)]
	public sealed class PredicateEntry
	{
		private PredicateType _type;

		public string            Type  { get; private set; }
		public string[]          Value { get; }
		public PropertiesDetails Details { get; }

		public PredicateEntry(string Type, string[] Value, PropertiesDetails Details)
		{
			this.Type = Type;
			this.Value = Value;
			this.Details = Details;

			CheckGeneralValidity();
			ConvertType();
			CheckValidityPerType();
		}

		private void CheckGeneralValidity()
		{
			if(string.IsNullOrWhiteSpace(Type))
				throw new BadCommandException();

			if(Value == null && Details == null)
				throw new BadCommandException();

			if(Value?.Length > 0 && Details != null)
				throw new BadCommandException();
		}

		private void ConvertType()
		{
			var type = Type.Trim();
			if(Enum.TryParse<PredicateType>(type, true, out _type) == false)
				throw new BadCommandException();
		}

		private void CheckValidityPerType()
		{
			switch (_type)
			{
				case PredicateType.Extension:
				case PredicateType.Regex: {
					if(Value?.Length == 0) throw new BadCommandException();

					if(Details != null) throw new BadCommandException();
				}
					break;
				case PredicateType.Properties: {
					if(Value?.Length > 0) throw new BadCommandException();

					if(Details == null) throw new BadCommandException();
				}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		[NotNull]
		public IFilePredicate CreatePredicate()
		{
			switch (_type)
			{
				case PredicateType.Extension: return new FileExtensionPredicate(Value);
				case PredicateType.Regex: return new FileNamePredicate(Value);
				case PredicateType.Properties: return new FilePropertiesPredicate(Details);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
