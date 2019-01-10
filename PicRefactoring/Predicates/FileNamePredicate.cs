using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using PicRefactoring.Abstractions;
using PicRefactoring.Commanding;

namespace PicRefactoring.Predicates
{
	public sealed class FileNamePredicate : IFilePredicate
	{
		private string[] _regExpressions;
		private Regex[] _regexes;

		public FileNamePredicate([NotNull] string[] regExpressions) 
		{
			_regExpressions = regExpressions;
		}

		public int Order => 1000;

		public void Prepare()
		{
			CheckValidity();
			CreateRegexes();
		}

		private void CheckValidity()
		{
			if(_regExpressions.Any(r => r.IsNullOrWhitespace()))
				throw new BadCommandException();
		}

		private void CreateRegexes()
		{
			_regexes = _regExpressions.Select(e => new Regex(e)).ToArray();
		}

		public bool FileMatches(IFileWrapper file)
		{
			var fileName = file.GetFileName();
			return _regexes.Any(r => r.IsMatch(fileName));
		}
	}
}
