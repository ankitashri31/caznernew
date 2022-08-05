using System.Collections.Generic;
using CaznerMarketplaceBackendApp.Auditing.Dto;
using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
