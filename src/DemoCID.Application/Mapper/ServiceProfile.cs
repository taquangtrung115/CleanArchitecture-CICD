using AutoMapper;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Product;
using DemoCICD.Domain.Entities;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;

namespace DemoCICD.Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // V1 Products
        CreateMap<Product, Response.ProductResponse>().ReverseMap();
        CreateMap<PagedResult<Product>, PagedResult<Response.ProductResponse>>().ReverseMap();

        // V2 Products
        CreateMap<Product, Contract.Services.V2.Product.Response.ProductResponse>().ReverseMap();

        // MotoGP Mappings
        ConfigureMotoGPMappings();
    }

    private void ConfigureMotoGPMappings()
    {
        // Rider mappings
        CreateMap<Rider, Contract.Services.V1.MotoGP.Rider.Response.RiderResponse>().ReverseMap();

        CreateMap<PagedResult<Rider>, PagedResult<Contract.Services.V1.MotoGP.Rider.Response.RiderResponse>>();

        CreateMap<RiderTeamHistory, Contract.Services.V1.MotoGP.Rider.Response.RiderTeamHistoryResponse>().ReverseMap();

        // Team mappings
        CreateMap<Team, Contract.Services.V1.MotoGP.Team.Response.TeamResponse>().ReverseMap();

        CreateMap<PagedResult<Team>, PagedResult<Contract.Services.V1.MotoGP.Team.Response.TeamResponse>>();

        CreateMap<Rider, Contract.Services.V1.MotoGP.Team.Response.TeamRiderResponse>().ReverseMap();

        CreateMap<Bike, Contract.Services.V1.MotoGP.Team.Response.TeamBikeResponse>();

        // Race mappings
        CreateMap<Race, Contract.Services.V1.MotoGP.Race.Response.RaceResponse>().ReverseMap();

        CreateMap<PagedResult<Race>, PagedResult<Contract.Services.V1.MotoGP.Race.Response.RaceResponse>>();

        CreateMap<Race, Contract.Services.V1.MotoGP.Race.Response.RaceWithResultsResponse>().ReverseMap();

        CreateMap<RaceEntry, Contract.Services.V1.MotoGP.Race.Response.RaceResultResponse>().ReverseMap();

        CreateMap<RaceEntry, Contract.Services.V1.MotoGP.Race.Response.RaceEntryResponse>().ReverseMap();

        // Season mappings
        CreateMap<Season, Contract.Services.V1.MotoGP.Season.Response.SeasonResponse>().ReverseMap();

        CreateMap<PagedResult<Season>, PagedResult<Contract.Services.V1.MotoGP.Season.Response.SeasonResponse>>().ReverseMap();
    }
}
