using System.Text.RegularExpressions;
using AutoMapper;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Task2.Contracts.Models;
using Task2.DataAccess.Sql;
using Task2.DataAccess.Sql.Entities;
using Task2.Services.Interfaces;
using Task2.Services.Mappers;

namespace Task2.Services.Services;

public class BalanceAccountService : IBalanceAccountService
{
    private readonly IMapper _mapper;
    private readonly BalanceAccountContext _context;

    public BalanceAccountService(BalanceAccountContext context)
    {
        _mapper = BalanceAccountMapperFactory.Create();
        _context = context;
    }

    public async Task FillDatabaseWithExcelAsync(string fileName, MemoryStream stream)
    {
        var excelFile = await AddExcelFileAsync(fileName);

        var balanceAccount = GetBalanceAccountList(stream, excelFile.FileId);

        await AddBalanceAccountsAsync(balanceAccount);
    }

    public async Task<IEnumerable<BalanceAccount>> GetBalanceAccountsAsync(int excelFileId)
    {
        var balanceAccountEntities = await _context.BalanceAccounts
            .Where(account => account.ExcelFileId == excelFileId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<BalanceAccount>>(balanceAccountEntities);
    }

    public async Task<IEnumerable<ExcelFile>> GetExcelFilesAsync()
    {
        var excelFileEntities = await _context.ExcelFiles.ToListAsync();
        return _mapper.Map<IEnumerable<ExcelFile>>(excelFileEntities);
    }

    private async Task AddBalanceAccountsAsync(IEnumerable<BalanceAccountEntity> balanceAccount)
    {
        var iterator = 0;
        foreach (var balanceAccounts in balanceAccount.GroupBy(_ => iterator++ / 100))
        {
            _context.BalanceAccounts.AddRange(balanceAccounts);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
            {
                Console.WriteLine(e);
            }
        }
    }

    private IEnumerable<BalanceAccountEntity> GetBalanceAccountList(Stream stream, int excelFileId)
    {
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var regexAccount = new Regex(@"^\d{4}$");
        var balanceAccount = new List<BalanceAccountEntity>();

        while (reader.Read())
        {
            int balanceAccountId;
            var balanceAccountIdValue = reader.GetValue(0);
            switch (balanceAccountIdValue)
            {
                case string value when regexAccount.IsMatch(value):
                    balanceAccountId = int.Parse(value);
                    break;
                case >= 1000 and <= 9999:
                    balanceAccountId = reader.GetInt32(0);
                    break;
                default:
                    continue;
            }

            try
            {
                balanceAccount.Add(new BalanceAccountEntity
                {
                    BalanceAccountId = balanceAccountId, ExcelFileId = excelFileId,
                    IncomingBalanceAsset = reader.GetDouble(1),
                    IncomingBalanceLiability = reader.GetDouble(2),
                    TurnoverAsset = reader.GetDouble(3),
                    TurnoverLiability = reader.GetDouble(4),
                    OutgoingBalanceAsset = reader.GetDouble(5),
                    OutgoingBalanceLiability = reader.GetDouble(6)
                });
            }
            catch (Exception e) when (e is ArgumentNullException or FormatException or OverflowException)
            {
                Console.WriteLine(e);
            }
        }

        return balanceAccount;
    }

    private async Task<ExcelFileEntity> AddExcelFileAsync(string fileName)
    {
        var excelFile = new ExcelFileEntity { FileName = fileName };
        _context.ExcelFiles.Add(excelFile);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
        {
            Console.WriteLine(e);
        }
        return excelFile;
    }
}