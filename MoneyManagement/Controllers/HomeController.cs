using Microsoft.AspNetCore.Mvc;
using MoneyManagement.Models;
using System.Diagnostics;
using OfficeOpenXml;
using System.Reflection;
using System.Xml.Linq;
using Spire.Xls.Core.Spreadsheet;
using Spire.Xls.Core;
using System.Collections.Generic;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Hosting;
using IWorkbook = NPOI.SS.UserModel.IWorkbook;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MoneyManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        _Context context = new _Context();

        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting)
        {
            _logger = logger;
            _hostingEnvironment = hosting;
        }

        public IActionResult Index()
        {
            List<Cards> select = context.CardTbl.ToList();
            ViewBag.List = select;
            return View();
        }

        public void createExcelFile()
        {
            HSSFWorkbook hssfwb = new HSSFWorkbook();
            ISheet sheet = hssfwb.CreateSheet("Sheet1");

            // Add header row with column names
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Id");
            headerRow.CreateCell(1).SetCellValue("Name");
            headerRow.CreateCell(2).SetCellValue("Price");
            headerRow.CreateCell(3).SetCellValue("CardNumber");
            headerRow.CreateCell(4).SetCellValue("Date");
            headerRow.CreateCell(5).SetCellValue("DateTimeSubmited");

            using (FileStream file = new FileStream(_hostingEnvironment.WebRootPath + "\\Data\\Data.xlsx", FileMode.Create))
            {
                hssfwb.Write(file);
            }
        }
        public bool AppendToExcel(ExcelData userData, string pathDoXls)
        {
            try
            {
                HSSFWorkbook hssfwb;
                using (FileStream file1 = new FileStream(pathDoXls, FileMode.Open, FileAccess.Read))
                {
                    hssfwb = new HSSFWorkbook(file1);
                }
                ISheet sheet = hssfwb.GetSheet("Sheet1");
                int lastRow = sheet.LastRowNum;

                IRow worksheetRow = sheet.CreateRow(lastRow + 1);

                ICell cell = worksheetRow.CreateCell(0);
                cell.SetCellValue(userData.Id.ToString());

                ICell cell1 = worksheetRow.CreateCell(1);
                cell1.SetCellValue(userData.Name);
                ICell cell2 = worksheetRow.CreateCell(2);
                cell2.SetCellValue(userData.Price.ToString());
                ICell cell3 = worksheetRow.CreateCell(3);
                cell3.SetCellValue(userData.CardNumber.ToString());
                ICell cell4 = worksheetRow.CreateCell(4);
                cell4.SetCellValue(userData.Date.ToString("MM/dd/yyyy"));
                ICell cell5 = worksheetRow.CreateCell(5);
                cell5.SetCellValue(userData.DateTimeSubmited.ToString("MM/dd/yyyy HH:mm:ss"));
                using (FileStream file = new FileStream(pathDoXls, FileMode.Create))
                {
                    hssfwb.Write(file);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public IActionResult addToList(string? Name, double? price, string? cardNumber, DateTime? date)
        {
            List<Cards> select = context.CardTbl.ToList();
            ViewBag.List = select;
            if (Name != null && price != null && cardNumber != null && date != null)
            {
                ExcelData ed = new ExcelData();
                ed.Id = Guid.NewGuid();
                ed.Name = Name;
                ed.Price = Convert.ToDouble(price);
                ed.CardNumber = cardNumber;
                ed.Date = Convert.ToDateTime(date);
                ed.DateTimeSubmited = DateTime.Now;
                if (AppendToExcel(ed, _hostingEnvironment.WebRootPath + "\\Data\\Data.xlsx"))
                {
                    return View("Index");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View("Index");
            }
        }
        public IActionResult watchData()
        {
            List<ExcelData> excelDataList = ReadDataFromExcel(_hostingEnvironment.WebRootPath + "\\Data\\Data.xlsx");
            return View(excelDataList);
        }
        private List<ExcelData> ReadDataFromExcel(string filePath)
        {
            List<ExcelData> excelDataList = new List<ExcelData>();

            if (System.IO.File.Exists(filePath))
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new HSSFWorkbook(fileStream);
                    ISheet sheet = workbook.GetSheetAt(0);

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ExcelData rowData = new ExcelData();

                        rowData.Id = Guid.Parse(row.GetCell(0).ToString());
                        rowData.Name = row.GetCell(1)?.ToString() ?? string.Empty;
                        rowData.Price = double.Parse(row.GetCell(2).ToString());
                        rowData.CardNumber = row.GetCell(3)?.ToString() ?? string.Empty;
                        rowData.Date = DateTime.Parse(row.GetCell(4).ToString());
                        rowData.DateTimeSubmited = DateTime.Parse(row.GetCell(5).ToString());
                        // خصوصیات دیگر بر اساس نیاز
                        excelDataList.Add(rowData);
                    }
                }
            }

            return excelDataList;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
