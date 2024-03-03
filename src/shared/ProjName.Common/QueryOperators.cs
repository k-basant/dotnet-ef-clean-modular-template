using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProjName.Common;

[JsonConverter(typeof(StringEnumConverter))]
public enum LogicalOperator
{
    And = 0,
    Or = 1,
}
[JsonConverter(typeof(StringEnumConverter))]
public enum ComparisonOperator
{
    Equals = 0,
    NotEquals = 1,
    GreaterThan = 2,
    LessThan = 3,
    GreaterThanOrEqual = 4,
    LessThanOrEqual = 5,
    Contains = 6,
}