using Abp.Application.Services;
using CaznerMarketplaceBackendApp.Artwork.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaznerMarketplaceBackendApp.Artwork
{
    public interface IArtworkAppService : IApplicationService
    {
        Task CreateArtWork(ArtworkDto createArtWork);
    }
}
