﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectRefsFixer
{
    public class Parameters
    {
        public string WorkingDirectory { get; }
        public string[] SourceUrls { get; }
        public bool FailOnNotFoundPackage { get; }
        public bool AllowPrereleasePackages { get; }
        public string[] Projects { get; }

        public Parameters(string[] args)
        {
            var positionalArgs = args.Where(x => !x.StartsWith("-")).ToArray();
            WorkingDirectory = positionalArgs.Length > 0 ? positionalArgs[0] : Environment.CurrentDirectory;
            SourceUrls = new[] {"https://api.nuget.org/v3/index.json"}.Concat(GetArgsByKey(args, "--source:")).ToArray();
            FailOnNotFoundPackage = !args.Contains("--ignoreMissingPackages");
            AllowPrereleasePackages = args.Contains("--allowPrereleasePackages");
            Projects = GetArgsByKey(args, "--project:").ToArray();
        }

        private static IEnumerable<string> GetArgsByKey(string[] args, string key)
        {
            return args.Where(x => x.StartsWith(key)).Select(x => x.Substring(key.Length).Trim());
        }
    }
}