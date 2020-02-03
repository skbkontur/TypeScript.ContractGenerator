using System;
using System.Collections.Generic;
using System.Linq;

using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

using NUnit.Framework;

using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public static class DiffAssertionExtensions
    {
        [NotNull]
        public static DiffResult Diff([CanBeNull] this string actual, [CanBeNull] string expected)
        {
            var diffBuilder = new InlineDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(expected, actual);
            if (diff.Lines.All(p => p.Type == ChangeType.Unchanged))
                return DiffResult.Equivalent;
            return new DiffResult(diff.Lines);
        }

        public static void ShouldBeEmpty([NotNull] this DiffResult result)
        {
            if (result == DiffResult.Equivalent) return;
            Assert.Fail($"Expected diff to be empty, but was:{Environment.NewLine}{result}");
        }
    }

    public class DiffResult
    {
        public DiffResult(IEnumerable<DiffPiece> lines = null)
        {
            this.lines = lines;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, lines?.Select(p =>
                {
                    switch (p.Type)
                    {
                    case ChangeType.Unchanged:
                        return p.Text;
                    case ChangeType.Deleted:
                        return $"- {RemoveFirstIfWhitespace(p.Text, 2)}";
                    case ChangeType.Inserted:
                        return $"+ {RemoveFirstIfWhitespace(p.Text, 2)}";
                    case ChangeType.Imaginary:
                    case ChangeType.Modified:
                        throw new NotSupportedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                    }
                }) ?? new string[0]);
        }

        [CanBeNull]
        private static string RemoveFirstIfWhitespace([CanBeNull] string s, int count)
        {
            if (!string.IsNullOrEmpty(s) && s.StartsWith(string.Join("", Enumerable.Repeat(" ", count))))
                return s.Substring(count);
            return s;
        }

        private readonly IEnumerable<DiffPiece> lines;

        public static readonly DiffResult Equivalent = new DiffResult();
    }
}