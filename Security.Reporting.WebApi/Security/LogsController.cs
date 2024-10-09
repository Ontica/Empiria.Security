/* Empiria Security ******************************************************************************************
*                                                                                                            *
*  Module   : Security Reporting Services                  Component : Web Api                               *
*  Assembly : Empiria.Security.Reporting.WebApi.dll        Pattern   : Query Controller                      *
*  Type     : LogsController                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api query methods that retrieve information about logs.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

namespace Empiria.Security.Reporting.WebApi {

  /// <summary>Web api query methods that retrieve information about logs.</summary>
  public class LogsController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v5/security/management/operational-logs/excel")]
    public SingleObjectModel ExportOperationalLogToExcel([FromBody] OperationalLogReportQuery query) {

      base.RequireBody(query);

      using (var service = OperationalLogService.UseCaseInteractor()) {

        FixedList<LogEntryDto> logEntries = service.GetLogEntries(query);

        var exporter = new LogExcelExporter(query, logEntries);

        FileDto report = exporter.Export();

        base.SetOperation($"Se exportó a Excel la bitácora de {GetOperationalLogName(query.OperationLogType)}.");

        return new SingleObjectModel(base.Request, report);
      }
    }

    #endregion Web Apis

    #region Helpers

    private string GetOperationalLogName(LogOperationType operationLogType) {
      switch (operationLogType) {
        case LogOperationType.Successful:
          return "Accesos exitosos";
        case LogOperationType.Error:
          return "Accesos no exitosos";
        case LogOperationType.PermissionsManagement:
          return "Gestión de permisos";
        case LogOperationType.UserManagement:
          return "Gestión de usuarios";
        default:
          return "No determinada";
      }
    }

    #endregion Helpers

  }  // class LogsController

}  // namespace Empiria.Security.Reporting.WebApi
