using System.Reflection;

namespace ProjName.Application.Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile() : this(Assembly.GetExecutingAssembly())
        {

        }
        public MappingProfile(Assembly assembly)
        {
            ApplyMappingsFromAssembly(assembly);
        }

        public void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && t.GetInterfaces().Any(i =>
                    i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) || i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("MapFrom") ?? type.GetInterfaces().LastOrDefault(x => x.Name == "IMapFrom`1")?.GetMethod("MapFrom");

                methodInfo?.Invoke(instance, new object[] { this });

                methodInfo = type.GetMethod("MapTo") ?? type.GetInterfaces().LastOrDefault(x => x.Name == "IMapTo`1")?.GetMethod("MapTo");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}