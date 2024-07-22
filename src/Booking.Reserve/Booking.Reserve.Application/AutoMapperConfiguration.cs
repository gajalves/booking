using AutoMapper;
using BooKing.Reserve.Application.Mappings;

namespace BooKing.Reserve.Application;
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
