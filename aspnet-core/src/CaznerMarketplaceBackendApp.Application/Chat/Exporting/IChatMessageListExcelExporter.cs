using System.Collections.Generic;
using Abp;
using CaznerMarketplaceBackendApp.Chat.Dto;
using CaznerMarketplaceBackendApp.Dto;

namespace CaznerMarketplaceBackendApp.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
