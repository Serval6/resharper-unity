using UnityEngine;

public class TopLevelClass
{
    private static readonly int Test = Shader.PropertyToID("test");

    public class NestedClass
    {
        private static string Test = "notTest";

        public void Method(Material material)
        {
            material.SetFloat("te{caret}st", 10.0f);
        }
    }
}