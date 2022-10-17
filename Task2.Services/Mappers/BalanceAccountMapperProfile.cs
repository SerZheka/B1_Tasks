using AutoMapper;
using Task2.Contracts.Models;
using Task2.DataAccess.Sql.Entities;

namespace Task2.Services.Mappers;

public class BalanceAccountMapperProfile : Profile
{
    public BalanceAccountMapperProfile()
    {
        CreateMap<BalanceAccount, BalanceAccountEntity>()
            .ForMember(dest => dest.ExcelFileId, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<ExcelFile, ExcelFileEntity>()
            .ReverseMap();
    }
}