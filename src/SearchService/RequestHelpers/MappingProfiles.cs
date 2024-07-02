using AutoMapper;

using Contracts;

using SearchService.Models;


namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Item>();
        // CreateMap<Contracts.AuctionUpdated, Models.Auction>();
        // CreateMap<Contracts.AuctionDeleted, Models.Auction>();
    }
}
