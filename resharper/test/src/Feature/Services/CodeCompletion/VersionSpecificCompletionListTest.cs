﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel.Propoerties;
using JetBrains.ProjectModel.Update;
using JetBrains.ReSharper.FeaturesTestFramework.Completion;
using JetBrains.Util;
using NUnit.Framework;
#if !RIDER
using PlatformID = JetBrains.Application.platforms.PlatformID;
#endif

#if RIDER
using JetBrains.Util.Dotnet.TargetFrameworkIds;
#endif

namespace JetBrains.ReSharper.Plugins.Unity.Tests.Feature.Services.CodeCompletion
{
    public abstract class VersionSpecificCompletionListTest : CodeCompletionTestBase
    {
        protected override CodeCompletionTestType TestType => CodeCompletionTestType.List;
        protected override string RelativeTestDataPath => @"codeCompletion\List";
        protected override bool CheckAutomaticCompletionDefault() => true;

#if RIDER
        protected override Pair<IProjectDescriptor, IList<Pair<IProjectReferenceDescriptor, IProjectReferenceProperties>>> CreateProjectDescriptor(string projectName, string outputAssemblyName, ICollection<FileSystemPath> absoluteFileSet,
            ICollection<KeyValuePair<TargetFrameworkId, IEnumerable<string>>> libraries, Guid projectGuid)
#else
                protected override Pair<IProjectDescriptor, IList<Pair<IProjectReferenceDescriptor, IProjectReferenceProperties>>> CreateProjectDescriptor(PlatformID platformID, string projectName, string outputAssemblyName, ICollection<FileSystemPath> absoluteFileSet, ICollection<KeyValuePair<TargetFrameworkId, IEnumerable<string>>> libraries, Guid projectGuid)
#endif
        {
#if RIDER
            var projectDescriptor = base.CreateProjectDescriptor(projectName, outputAssemblyName, absoluteFileSet, libraries, projectGuid);
#else
            var projectDescriptor = base.CreateProjectDescriptor(platformID, projectName, outputAssemblyName, absoluteFileSet, libraries, projectGuid);
#endif
            var activeConfigurations = projectDescriptor.First.ProjectProperties.ActiveConfigurations;
            var projectConfiguration = (CSharpProjectConfiguration)activeConfigurations.GetOrCreateConfiguration(TargetFrameworkId.Default);
            var testUnityAttributes = GetClassAttributes<TestUnityAttribute>().Single();
            projectConfiguration.DefineConstants = testUnityAttributes.DefineConstants;
            return projectDescriptor;
        }
    }

    // These two tests have different parameters in different versions, make sure
    // we using the right version
    [TestUnity(UnityVersion.Unity54)]
    public class Unity54CompletionListTest : VersionSpecificCompletionListTest
    {
        [Test] public void OnParticleTriggerWithOneArg54() { DoNamedTest(); }
    }

    [TestUnity(UnityVersion.Unity55)]
    public class Unity55CompletionListTest : VersionSpecificCompletionListTest
    {
        [Test] public void OnParticleTriggerWithNoArgs55() { DoNamedTest(); }
    }
}