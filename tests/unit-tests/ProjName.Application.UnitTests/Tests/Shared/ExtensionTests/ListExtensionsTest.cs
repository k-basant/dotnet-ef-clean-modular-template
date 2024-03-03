using ProjName.Application.Shared.Extensions;
using ProjName.Application.UnitTests.Setup;

namespace ProjName.Application.UnitTests.Tests.Shared.ExtensionTests;

public class ListExtensionsTest : TestBase
{
    public ListExtensionsTest(StartupFixture fixture) : base(fixture)
    {
    }

    [Theory]
    [MemberData(nameof(ListExtensionsTest.ListDiffStringData))]
    public void ListDifference_ReturnsAddUpdateAndDeleteDifferences_WhenInvokedWithStringList(List<string> existingData, List<string> newData, List<string> expectedAdd, List<string> expectedUpdate, List<string> expectedDel)
    {
        (var actualAdd, var actualUpdate, var actualDel) = existingData.ListDifference(newData, (x1, x2) => x1 == x2);

        actualAdd.Should().BeEquivalentTo(expectedAdd);
        actualUpdate.Should().BeEquivalentTo(expectedUpdate);
        actualDel.Should().BeEquivalentTo(expectedDel);
    }

    #region Test Data
    public static IEnumerable<object[]> ListDiffStringData()
    {
        yield return new object[] { new List<string> { "A", "B" }, new List<string> { "B", "C" }, new List<string> { "C" }, new List<string> { "B" }, new List<string> { "A" } };
        yield return new object[] { new List<string> { "A", "B" }, new List<string> { "C", "D" }, new List<string> { "C", "D" }, new List<string> { }, new List<string> { "A", "B" } };
        yield return new object[] { new List<string>(), new List<string> { "A", "B" }, new List<string> { "A", "B" }, new List<string> { }, new List<string> { } };
        yield return new object[] { new List<string> { "A", "B" }, new List<string>(), new List<string> { }, new List<string> { }, new List<string> { "A", "B" } };
    }
    #endregion
}
