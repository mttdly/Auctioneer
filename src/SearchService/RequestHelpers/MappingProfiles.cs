using AutoMapper;

using Contracts;
using SearchService.Consumers;
using SearchService.Models;


namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Item>();
        CreateMap<AuctionUpdated, Item>();
        // CreateMap<Contracts.AuctionUpdated, Models.Auction>();
        // CreateMap<Contracts.AuctionDeleted, Models.Auction>();
    }
}
