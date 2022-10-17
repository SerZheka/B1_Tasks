using AutoMapper;

namespace Task2.Services.Mappers;

public static class BalanceAccountMapperFactory
{
    private static readonly IMapper Mapper;

    static BalanceAccountMapperFactory()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BalanceAccountMapperProfile>();
            cfg.AllowNullCollections = true;
        });
        
        configuration.AssertConfigurationIsValid();
        Mapper = configuration.CreateMapper();
    }

    public static IMapper Create()
    {
        return Mapper;
    }
}