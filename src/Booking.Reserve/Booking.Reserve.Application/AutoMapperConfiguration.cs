using AutoMapper;
using Booking.Reserve.Application.Mappings;

namespace Booking.Reserve.Application;
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
