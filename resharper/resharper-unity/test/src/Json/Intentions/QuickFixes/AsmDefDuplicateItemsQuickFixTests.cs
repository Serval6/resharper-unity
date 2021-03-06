﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel.Update;
using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using JetBrains.ReSharper.Plugins.Unity.Json.Feature.Services.QuickFixes;
using JetBrains.ReSharper.TestFramework;
using JetBrains.Util;
using JetBrains.Util.Dotnet.TargetFrameworkIds;
using NUnit.Framework;

namespace JetBrains.ReSharper.Plugins.Unity.Tests.Json.Intentions.QuickFixes
{
    [TestUnity]
    [TestFileExtension(".asmdef")]
    public class AsmDefDuplicateItemsQuickFixAvailabilityTests : QuickFixAvailabilityTestBase
    {
        protected override string RelativeTestDataPath => @"Json\Intentions\QuickFixes\DuplicateItems\Availability";

        [Test] public void Test01() { DoNamedTest("Test01_SecondProject.asmdef"); }
        [Test] public void Test02() { DoNamedTest("Test02_SecondProject.asmdef"); }

        // If we don't have valid (but duplicated references), the invalid reference error trumps the duplicate item warning
        protected override TestSolutionConfiguration CreateSolutionConfiguration(
            ICollection<KeyValuePair<TargetFrameworkId, IEnumerable<string>>> referencedLibraries,
            IEnumerable<string> fileSet)
        {
            if (fileSet == null)
                throw new ArgumentNullException(nameof(fileSet));

            var mainProjectFileSet = fileSet.Where(filename => !filename.Contains("_SecondProject"));
            var mainAbsoluteFileSet = mainProjectFileSet.Select(path => TestDataPath.Combine(path)).ToList();

            var descriptors = new Dictionary<IProjectDescriptor, IList<Pair<IProjectReferenceDescriptor, IProjectReferenceProperties>> >();

            var mainDescriptorPair = CreateProjectDescriptor(ProjectName, ProjectName, mainAbsoluteFileSet,
                referencedLibraries, ProjectGuid);
            descriptors.Add(mainDescriptorPair.First, mainDescriptorPair.Second);

            var referencedProjectFileSet = fileSet.Where(filename => filename.Contains("_SecondProject")).ToList();
            if (Enumerable.Any(referencedProjectFileSet))
            {
                var secondAbsoluteFileSet =
                    referencedProjectFileSet.Select(path => TestDataPath.Combine(path)).ToList();
                var secondProjectName = "Second_" + ProjectName;
                var secondDescriptorPair = CreateProjectDescriptor(secondProjectName, secondProjectName,
                    secondAbsoluteFileSet, referencedLibraries, SecondProjectGuid);
                descriptors.Add(secondDescriptorPair.First, secondDescriptorPair.Second);
            }

            return new TestSolutionConfiguration(FileSystemPath.Empty, descriptors);
        }
    }

    [TestUnity]
    [TestFileExtension(".asmdef")]
    public class AsmDefDuplicateItemsQuickFixTests : QuickFixTestBase<AsmDefRemoveInvalidArrayItemQuickFix>
    {
        protected override string RelativeTestDataPath => @"Json\Intentions\QuickFixes\DuplicateItems";

        [Test] public void Test01() { DoNamedTest(); }
        [Test] public void Test02() { DoNamedTest(); }
    }
}