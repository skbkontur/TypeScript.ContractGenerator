using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.Build.Construction;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace ProjectRefsFixer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var parameters = new Parameters(args);

            Console.WriteLine($"Converting project references to NuGet package references for all projects of solutions located in '{parameters.WorkingDirectory}'.");

            var solutionFiles = Directory.GetFiles(parameters.WorkingDirectory, "*.sln");
            if (solutionFiles.Length == 0)
            {
                Console.WriteLine("No solution files found.");
                return;
            }

            Console.WriteLine($"Found solution files:");
            Console.WriteLine($"\t{string.Join(Environment.NewLine + "\t", solutionFiles)}");
            Console.WriteLine();

            foreach (var solutionFile in solutionFiles)
                HandleSolution(solutionFile, parameters);
        }

        private static void HandleSolution(string solutionFile, Parameters parameters)
        {
            var solutionName = Path.GetFileName(solutionFile);
            var projects = SolutionFile.Parse(solutionFile).ProjectsInOrder;
            if (!projects.Any())
            {
                Console.WriteLine($"No projects found in solution {solutionName}.");
                return;
            }

            Console.WriteLine($"Found projects in solution {solutionName}:");
            Console.WriteLine($"\t{string.Join(Environment.NewLine + "\t", projects.Select(project => project.AbsolutePath))}");
            Console.WriteLine();

            if (parameters.Projects.Any())
            {
                projects = projects.Where(x => parameters.Projects.Contains(x.ProjectName)).ToArray();

                Console.WriteLine($"Filtered projects in solution {solutionName}:");
                Console.WriteLine($"\t{string.Join(Environment.NewLine + "\t", projects.Select(project => project.AbsolutePath))}");
                Console.WriteLine();
            }

            var allProjectsInSolution = projects.Select(p => p.ProjectName).ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var solutionProject in projects)
                HandleProject(solutionProject, allProjectsInSolution, parameters);
        }

        private static void HandleProject(ProjectInSolution solutionProject, ISet<string> allProjectsInSolution, Parameters parameters)
        {
            if (!File.Exists(solutionProject.AbsolutePath))
            {
                Console.WriteLine($"Project '{solutionProject.AbsolutePath}' doesn't exists.");
                return;
            }

            Console.WriteLine($"Working with project '{solutionProject.ProjectName}'..");

            var project = Project.FromFile(solutionProject.AbsolutePath, new ProjectOptions
                {
                    LoadSettings = ProjectLoadSettings.IgnoreMissingImports
                });

            if (ShouldIgnore(project))
            {
                Console.WriteLine($"Ignore project  '{solutionProject.ProjectName}' due to ProjectRefsFixerExclude property in csproj.");
                return;
            }

            var references = FindLocalProjectReferences(project, allProjectsInSolution);
            if (!references.Any())
            {
                Console.WriteLine($"No references found in project {solutionProject.ProjectName}.");
                return;
            }

            Console.WriteLine($"Found references in {solutionProject.ProjectName}:");
            Console.WriteLine($"\t{string.Join(Environment.NewLine + "\t", references.Select(GetProjectName))}");
            Console.WriteLine();

            var allowPrereleasePackages = parameters.AllowPrereleasePackages;
            Console.WriteLine(allowPrereleasePackages ? "Will allow prerelease versions in package references." : "Won't allow prerelease versions in package.");
            Console.WriteLine();

            foreach (var reference in references)
                HandleReference(project, reference, allowPrereleasePackages, parameters);

            project.Save();

            Console.WriteLine();
        }

        private static bool ShouldIgnore(Project project)
        {
            return project.Properties.Any(item => item.Name == "ProjectRefsFixerExclude" && item.EvaluatedValue == "true");
        }

        private static List<ProjectItem> FindLocalProjectReferences(Project project, ISet<string> localProjects)
        {
            return project.Items
                          .Where(item => item.ItemType == "ProjectReference")
                          .Where(item => localProjects.Contains(GetProjectName(item)))
                          .ToList();
        }

        private static void HandleReference(Project project, ProjectItem reference, bool allowPrereleasePackages,
                                            Parameters parameters)
        {
            var packageName = "SkbKontur." + GetProjectName(reference);
            var packageVersion = GetLatestNugetVersion(packageName, allowPrereleasePackages, parameters.SourceUrls);
            if (packageVersion == null)
            {
                if (parameters.FailOnNotFoundPackage)
                    throw new Exception($"No versions of package '{packageName}' were found on '{string.Join(", ", parameters.SourceUrls)}'.");
                return;
            }

            Console.WriteLine($"Latest version of NuGet package '{packageName}' is '{packageVersion}'");

            project.RemoveItem(reference);

            Console.WriteLine($"Removed project reference to '{reference.EvaluatedInclude}'.");

            project.AddItem("PackageReference", packageName, new[]
                {
                    new KeyValuePair<string, string>("Version", packageVersion.ToString())
                });

            Console.WriteLine($"Added package reference to '{packageName}' of version '{packageVersion}'.");
            Console.WriteLine();
        }

        private static NuGetVersion GetLatestNugetVersion(string package, bool includePrerelease, string[] sourceUrls)
        {
            foreach (var source in sourceUrls)
            {
                var latestVersion = GetLatestNugetVersion(package, includePrerelease, source);
                if (latestVersion != null)
                    return latestVersion;
            }

            return null;
        }

        private static NuGetVersion GetLatestNugetVersion(string package, bool includePrerelease, string sourceUrl)
        {
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(Repository.Provider.GetCoreV3());

            var sourceRepository = new SourceRepository(new PackageSource(sourceUrl), providers);
            var metadataResource = sourceRepository.GetResource<PackageMetadataResource>();

            var versions = metadataResource.GetMetadataAsync(package, includePrerelease, false, new SourceCacheContext(), new NullLogger(), CancellationToken.None)
                                           .GetAwaiter()
                                           .GetResult()
                                           .Where(data => data.Identity.Id == package)
                                           .Select(data => data.Identity.Version)
                                           .ToArray();

            return versions.Any() ? versions.Max() : null;
        }

        private static string GetProjectName(ProjectItem reference)
        {
            return Path.GetFileName(reference.EvaluatedInclude).Replace(".csproj", "");
        }
    }
}