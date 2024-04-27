using Microsoft.Extensions.Logging;
using Seagull.BarTender.Print;
using SMT.Access.Repository.Interfaces;
using SMT.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class PrinterService : IPrinterService
    {
        private readonly IComponentRepository _componentRepository;
        private readonly ILogger<PrinterService> _logger;

        public PrinterService(ILogger<PrinterService> logger, IComponentRepository componentRepository)
        {
            _logger = logger;
            _componentRepository = componentRepository;
        }

        public async Task Print(string partNumber)
        {
            try
            {
                var compnent = await _componentRepository.FindAsync(x => x.PartNumber == partNumber);

                using var engine = new Engine(true);
                engine.Start();

                LabelFormatDocument btformate = engine.Documents.Open(@"d:\bmw.btw", "Xprinter XP-370B");

                btformate.SubStrings["QR"].Value = $"{compnent.PartNumber}@{compnent.RCode}@{compnent.StorePlaceNumber}@{compnent.SapPlace}@{compnent.PlaceCode}";
                btformate.SubStrings["PartNumber"].Value = compnent.PartNumber;
                btformate.SubStrings["Rcode"].Value = compnent.RCode;
                btformate.SubStrings["PlaceCode"].Value = compnent.PlaceCode;
                btformate.SubStrings["SapPlace"].Value = compnent.SapPlace;
                btformate.SubStrings["StorePlace"].Value = compnent.StorePlaceNumber;
                btformate.SubStrings["Spec"].Value = compnent.Specification;

                Result result = btformate.Print("PrintJob1", out Messages messages);

                engine.Stop();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while printing a QR");

                //throw;
            }
            
        }
    }
}
