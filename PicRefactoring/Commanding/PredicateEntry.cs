﻿using System;
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
	public class PredicateEntry
	{
		private PredicateType _type;

		public string            Type  { get; private set; }
		public string[]          Value { get; }
		public PropertiesDetails Details { get; }

		public PredicateEntry() {}

		public PredicateEntry(string Type, string[] Value, PropertiesDetails Details)
		{
			this.Type = Type;
			this.Value = Value;
			this.Details = Details;
		}

		public virtual void CheckValidity()
		{
			CheckGeneralValidity();
			ConvertType();
			CheckValidityPerType();
		}

		private void CheckGeneralValidity()
		{
			if(Type.IsNullOrWhitespace())
				throw new BadCommandException("predicate Type");

			if(Value == null && Details == null)
				throw new BadCommandException("Value and details null");

			if(Value?.Length > 0 && Details != null)
				throw new BadCommandException("Value can't coexists with Details");
		}

		private void ConvertType()
		{
			var type = Type.Trim();
			if(Enum.TryParse<PredicateType>(type, true, out _type) == false)
				throw new BadCommandException($"Type {Type} not supported");
		}

		private void CheckValidityPerType()
		{
			switch (_type)
			{
				case PredicateType.Extension:
				case PredicateType.Regex: {
					if(Value == null || Value.Length == 0) throw new BadCommandException($"Type {_type} requires Values");
				}
					break;
				case PredicateType.Properties: {
					if(Details == null) throw new BadCommandException($"Type {_type} requires Details");
				}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		[NotNull]
		public virtual IFilePredicate CreatePredicate()
		{
			var predicate = Create();
			predicate.Prepare();
			return predicate;
		}

		private IFilePredicate Create()
		{
			switch (_type)
			{
				case PredicateType.Extension:  return new FileExtensionPredicate(Value);
				case PredicateType.Regex:      return new FileNamePredicate(Value);
				case PredicateType.Properties: return new FilePropertiesPredicate(Details);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
