using AutoMapper;
using BooKing.Identity.Application.Mappings;

namespace BooKing.Identity.Application;
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