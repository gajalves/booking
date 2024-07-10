using AutoMapper;
using BooKing.Apartments.Application.Mappings;

namespace BooKing.Apartments.Application;
public static class AutoMapperConfiguration
{
    public static MapperConfiguration Create()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AddProfile(new MappingProfile());
        });

        return mapperConfig;

    }
}
