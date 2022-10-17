using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Task2.Contracts.Models;
using Task2.Models;
using Task2.Services.Interfaces;

namespace Task2.Controllers;

public class HomeController : Controller
{
    private readonly IBalanceAccountService _balanceAccountService;

    public HomeController(IBalanceAccountService balanceAccountService)
    {
        _balanceAccountService = balanceAccountService;
    }

    public async Task<IActionResult> Index()
    {
        var excelFiles = await _balanceAccountService.GetExcelFilesAsync();
        return View(excelFiles);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Upload(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var fileName = WebUtility.HtmlEncode(file.FileName);
        
        await _balanceAccountService.FillDatabaseWithExcelAsync(fileName, stream);

        return Redirect("/");
    }

    public async Task<IActionResult> FileDetails(ExcelFile excelFile)
    {
        ViewBag.FileName = excelFile.FileName;
        var balanceAccounts = await _balanceAccountService.GetBalanceAccountsAsync(excelFile.FileId);
        return View(balanceAccounts);
    }
}