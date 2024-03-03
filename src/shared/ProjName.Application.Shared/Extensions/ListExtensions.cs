namespace ProjName.Application.Shared.Extensions;

public static class ListExtensions
{
    public static (List<T2> Added, List<T2> Updated, List<T1> Deleted) ListDifference<T1, T2>(this List<T1> oldData, List<T2> newData, Func<T1, T2, bool> equalityCheck)
    {
        var addedValues = newData.Where(l1 => !oldData.Any(l2 => equalityCheck(l2, l1))).ToList();
        var updatedValues = newData.Where(l1 => oldData.Any(l2 => equalityCheck(l2, l1))).ToList();
        var deletedValues = oldData.Where(l1 => !newData.Any(l2 => equalityCheck(l1, l2))).ToList();

        return (addedValues, updatedValues, deletedValues);
    }
}
