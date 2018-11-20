using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Plugins.Unity.CSharp.Daemon.Errors;
using JetBrains.ReSharper.Plugins.Unity.CSharp.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Plugins.Unity.CSharp.Daemon.Stages.Highlightings;
#if RIDER
using JetBrains.ReSharper.Plugins.Unity.Rider.CodeInsights;
#endif
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace JetBrains.ReSharper.Plugins.Unity.CSharp.Daemon.Stages.GutterMarks
{
    [ElementProblemAnalyzer(typeof(IFieldDeclaration), HighlightingTypes = new[]
    {
#if RIDER
            typeof(UnityCodeInsightsHighlighting)
#else
        typeof(UnityGutterMarkInfo),
#endif
    })]
    public class UnityFieldDetector : UnityElementProblemAnalyzer<IFieldDeclaration>
    {
        private readonly UnityImplicitUsageHighlightingContributor myImplicitUsageHighlightingContributor;

        public UnityFieldDetector(UnityApi unityApi, UnityImplicitUsageHighlightingContributor implicitUsageHighlightingContributor)
            : base(unityApi)
        {
            myImplicitUsageHighlightingContributor = implicitUsageHighlightingContributor;
        }

        protected override void Analyze(IFieldDeclaration element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var field = element.DeclaredElement;
            if (field != null && Api.IsSerialisedField(field))
            {
                myImplicitUsageHighlightingContributor.AddUnityImplicitFieldUsage(consumer, element, "This field is initialised by Unity");
            }
        }
    }
}