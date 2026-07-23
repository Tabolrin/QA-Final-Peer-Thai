using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class InfrastructureSmokeTests
{
    [UnityTest]
    public IEnumerator PlayModeTestRunner_IsWorking()
    {
        yield return null;
        Assert.Pass();
    }
}
