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
        CreateMap<Rider, Contract.Services.V1.MotoGP.Rider.Response.RiderResponse>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.GetAge()))
            .ForMember(dest => dest.YearsInMotoGP, opt => opt.MapFrom(src => src.GetYearsInMotoGP()))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Nationality.Code))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Nationality.Name))
            .ForMember(dest => dest.CountryFlag, opt => opt.MapFrom(src => src.Nationality.Flag))
            .ForMember(dest => dest.CurrentTeamName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<PagedResult<Rider>, PagedResult<Contract.Services.V1.MotoGP.Rider.Response.RiderResponse>>();

        CreateMap<RiderTeamHistory, Contract.Services.V1.MotoGP.Rider.Response.RiderTeamHistoryResponse>()
            .ForMember(dest => dest.TeamName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.SeasonYear, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.IsCurrentTeam, opt => opt.MapFrom(src => !src.EndDate.HasValue));

        // Team mappings
        CreateMap<Team, Contract.Services.V1.MotoGP.Team.Response.TeamResponse>()
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country.Code))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.ActiveRidersCount, opt => opt.MapFrom(src => src.GetActiveRidersCount()))
            .ForMember(dest => dest.BikesCount, opt => opt.MapFrom(src => src.GetBikesCount()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<PagedResult<Team>, PagedResult<Contract.Services.V1.MotoGP.Team.Response.TeamResponse>>();

        CreateMap<Rider, Contract.Services.V1.MotoGP.Team.Response.TeamRiderResponse>()
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Nationality.Code));

        CreateMap<Bike, Contract.Services.V1.MotoGP.Team.Response.TeamBikeResponse>();

        // Race mappings
        CreateMap<Race, Contract.Services.V1.MotoGP.Race.Response.RaceResponse>()
            .ForMember(dest => dest.SeasonYear, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country.Code))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.FreeP1Date, opt => opt.MapFrom(src => src.Schedule.Practice1))
            .ForMember(dest => dest.FreeP2Date, opt => opt.MapFrom(src => src.Schedule.Practice2))
            .ForMember(dest => dest.QualifyingDate, opt => opt.MapFrom(src => src.Schedule.Qualifying))
            .ForMember(dest => dest.WarmUpDate, opt => opt.MapFrom(src => src.Schedule.Practice3))
            .ForMember(dest => dest.TotalEntries, opt => opt.MapFrom(src => src.GetTotalEntries()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<PagedResult<Race>, PagedResult<Contract.Services.V1.MotoGP.Race.Response.RaceResponse>>();

        CreateMap<Race, Contract.Services.V1.MotoGP.Race.Response.RaceWithResultsResponse>()
            .ForMember(dest => dest.SeasonYear, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country.Code))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.RaceEntries.Where(e => e.Result != null).OrderBy(e => e.Result!.Position)));

        CreateMap<RaceEntry, Contract.Services.V1.MotoGP.Race.Response.RaceResultResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RaceId, opt => opt.MapFrom(src => src.RaceId))
            .ForMember(dest => dest.RiderId, opt => opt.MapFrom(src => src.RiderId))
            .ForMember(dest => dest.RiderName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.RiderNumber, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.TeamId))
            .ForMember(dest => dest.TeamName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.BikeId, opt => opt.MapFrom(src => src.BikeId))
            .ForMember(dest => dest.BikeModel, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.StartingPosition, opt => opt.MapFrom(src => src.StartingGrid))
            .ForMember(dest => dest.FinishPosition, opt => opt.MapFrom(src => src.Result != null ? src.Result.Position : (int?)null))
            .ForMember(dest => dest.FinishTime, opt => opt.MapFrom(src => src.Result != null ? src.Result.RaceTime : (TimeSpan?)null))
            .ForMember(dest => dest.BestLapTime, opt => opt.MapFrom(src => src.FastestLapTime))
            .ForMember(dest => dest.PointsEarned, opt => opt.MapFrom(src => src.Result != null ? src.Result.Points : (int?)null))
            .ForMember(dest => dest.IsFinisher, opt => opt.MapFrom(src => src.Result != null && src.Result.IsFinisher))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Result != null ? src.Result.Reason : src.Notes));

        CreateMap<RaceEntry, Contract.Services.V1.MotoGP.Race.Response.RaceEntryResponse>()
            .ForMember(dest => dest.RiderName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.RiderNumber, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.TeamName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.BikeModel, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.StartingPosition, opt => opt.MapFrom(src => src.StartingGrid));

        // Season mappings
        CreateMap<Season, Contract.Services.V1.MotoGP.Season.Response.SeasonResponse>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.StartDate <= DateTime.Now && src.EndDate >= DateTime.Now))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.EndDate < DateTime.Now))
            .ForMember(dest => dest.ChampionRiderName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.ChampionTeamName, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.TotalRaces, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.CompletedRaces, opt => opt.Ignore()) // Will be handled in query handler
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<PagedResult<Season>, PagedResult<Contract.Services.V1.MotoGP.Season.Response.SeasonResponse>>();
    }
}
