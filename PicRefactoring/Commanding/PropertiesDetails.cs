﻿using System;
using JetBrains.Annotations;
using JsonRazor.Serialization;
using PicRefactoring.Abstractions;

namespace PicRefactoring.Commanding
{
	public enum ComparisonType
	{
		OR,
		AND
	}


	[JsonInfo(ModelMembers.Property, Flags = Deserializer.DefaultFlags)]
	public class PropertiesDetails
	{
		private const int MinWeightKb  = 1;
		private const int MinSizePixel = 31;
		private Func<bool,bool,bool> _comparison;

		public string Comparison { get; private set; }
		public int    MinKbWeight  { get; private set; }
		public int    MinSize    { get; private set; }

		public PropertiesDetails() {}

		public PropertiesDetails(string comparison, int minKbWeight, int minSize)
		{
			Comparison = comparison;
			MinKbWeight = minKbWeight;
			MinSize = minSize;
		}

		public void Prepare()
		{
			if(MinSize < MinSizePixel || MinKbWeight < MinWeightKb || Comparison == null)
				throw new BadCommandException();

			var comp = Comparison.ToUpper().Trim();
			if(! Enum.TryParse(typeof(ComparisonType), comp, out object compType))
				throw new BadCommandException();

			SetComparisonFunc(compType);
		}

		private void SetComparisonFunc(object compType)
		{
			var comparison = (ComparisonType)compType;
			switch (comparison) 
			{
				case ComparisonType.OR:
					_comparison = (b1, b2) => b1 || b2;
					break;
				case ComparisonType.AND:
					_comparison = (b1, b2) => b1 && b2;
					break;
			}
		}

		public bool FileMatches([NotNull] IFileWrapper file)
		{
			var width = file.GetWidth();
			var height = file.GetHeight();
			var maxSize = Math.Max(width, height);
			var weight = file.GetWeightInKb();

			return EvaluateValues(maxSize, weight);
		}

		private bool EvaluateValues(int maxSize, int weight)
		{
			return _comparison(maxSize >= MinSize, weight >= MinKbWeight);
		}
	}
}
