using PandaFileExporter;
using PandaTech.ServiceResponse;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Filters;

public class HelperMethods
{
    public static byte[] Export<T>(List<T> itemsToExport, ExportType exportType) where T : class
    {

        byte[] exportData;

        switch (exportType)
        {
            case ExportType.CSV:
                exportData = FileExporter.ToCsvArray(itemsToExport);
                break;
            case ExportType.PDF:
                exportData = FileExporter.ToPdfArray(itemsToExport);
                break;
            case ExportType.XLSX:
                exportData = FileExporter.ToExcelArray(itemsToExport);
                break;
            default:
                throw new BadRequestException("not_supported_export_type");
        }

        return exportData;
    }

}