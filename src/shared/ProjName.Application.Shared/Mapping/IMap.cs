using AutoMapper;
using ProjName.Common;

namespace ProjName.Application.Shared;

public interface IMapFrom<T>
{
    void MapFrom(Profile profile)
    {
        var map = profile.CreateMap(typeof(T), GetType());
        if (typeof(BaseEntity<Guid>).IsAssignableFrom(typeof(T)))
        {
            if (typeof(T).GetProperty("CreatedBy") != null && this.GetType().GetProperty("CreatedByUserName") != null)
            {
                map.ForMember("CreatedByUserName", o => o.MapFrom("CreatedBy.DisplayName"));
            }
            if (typeof(T).GetProperty("UpdatedBy") != null && this.GetType().GetProperty("UpdatedByUserName") != null)
            {
                map.ForMember("UpdatedByUserName", o => o.MapFrom("UpdatedBy.DisplayName"));
            }
        }
    }
}
public interface IMapTo<T>
{
    void MapTo(Profile profile) => profile.CreateMap(GetType(), typeof(T));
}
