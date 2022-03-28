using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WarehouseAPI.DTOs;
using WarehouseAPI.Entities;

namespace WarehouseAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<StorageDTO, Storage>().ReverseMap();
        CreateMap<StorageCreationDTO, Storage>();
        CreateMap<ItemCreationDTO, Item>().ForMember(x => x.Image,
            options => options.Ignore());
        CreateMap<Item, ItemDTO>();
        CreateMap<IdentityUser, UserDTO>();
    }


}