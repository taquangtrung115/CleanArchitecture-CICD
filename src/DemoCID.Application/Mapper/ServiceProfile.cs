using AutoMapper;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Services.V1.Product;
using DemoCICD.Domain.Entities;
using DemoCICD.Domain.Entities.MotoGP.TeamRiderManagement;
using DemoCICD.Domain.Entities.MotoGP.RaceManagement;
using DemoCICD.Domain.Entities.RentalRoom.Rooms;
using DemoCICD.Domain.Entities.RentalRoom.Locations;
using DemoCICD.Domain.Entities.RentalRoom.Bills;
using RentalProfile = DemoCICD.Domain.Entities.RentalRoom.Profiles.Profile;

namespace DemoCICD.Application.Mapper;

public class ServiceProfile : AutoMapper.Profile
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

        // RentalRoom Mappings
        ConfigureRentalRoomMappings();
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

    private void ConfigureRentalRoomMappings()
    {
        // Room mappings
        CreateMap<Room, Contract.Services.V1.RentalRoom.RoomResponse.Response>()
            .ForMember(dest => dest.LocationAddress, opt => opt.MapFrom(src => src.Location != null ? src.Location.FullAddress : null))
            .ReverseMap();

        CreateMap<PagedResult<Room>, PagedResult<Contract.Services.V1.RentalRoom.RoomResponse.Response>>();

        // Profile mappings (using alias to avoid conflict)
        CreateMap<RentalProfile, Contract.Services.V1.RentalRoom.ProfileResponse.Response>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomNumber : null))
            .ReverseMap();

        CreateMap<PagedResult<RentalProfile>, PagedResult<Contract.Services.V1.RentalRoom.ProfileResponse.Response>>();

        // Location mappings
        CreateMap<Location, Contract.Services.V1.RentalRoom.LocationResponse.Response>().ReverseMap();

        CreateMap<PagedResult<Location>, PagedResult<Contract.Services.V1.RentalRoom.LocationResponse.Response>>();

        // Bill mappings
        CreateMap<Bill, Contract.Services.V1.RentalRoom.BillResponse.Response>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomNumber : "Unknown"))
            .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.FullName : "Unknown"))
            .ReverseMap();

        CreateMap<PagedResult<Bill>, PagedResult<Contract.Services.V1.RentalRoom.BillResponse.Response>>();

        // BillDetail mappings
        CreateMap<BillDetail, Contract.Services.V1.RentalRoom.BillResponse.DetailItemResponse>().ReverseMap();

        // Bill detailed response (with BillDetails)
        CreateMap<Bill, Contract.Services.V1.RentalRoom.BillResponse.DetailedResponse>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null ? src.Room.RoomNumber : "Unknown"))
            .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.Profile != null ? src.Profile.FullName : "Unknown"))
            .ForMember(dest => dest.BillDetails, opt => opt.MapFrom(src => src.BillDetails))
            .ReverseMap();
    }
}
