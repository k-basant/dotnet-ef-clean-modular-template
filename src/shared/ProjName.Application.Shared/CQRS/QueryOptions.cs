using ProjName.Common;

namespace ProjName.Application.Shared.CQRS
{
    public class GetAllQueryOptions
    {
        public bool IsDetailed { get; set; }
        public int PageNum { get; set; } = -1; // Disable pagination by default
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public LogicalOperator CombineWith { get; set; } = LogicalOperator.And;
    }
    public class FilterExp<T>
    {
        public LogicalOperator CombineWith { get; set; } = LogicalOperator.And;
        public List<FilterOption<T>> Filters { get; set; }
    }
    public class FilterOption<T>
    {
        public T Value { get; set; }
        public ComparisonOperator ComparisonOperator { get; set; }
    }
}
