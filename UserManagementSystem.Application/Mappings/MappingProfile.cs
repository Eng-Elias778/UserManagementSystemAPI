using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.UserPermissions.Select(up => up.Permission.PermissionName)));

            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.UserPermissions, opt => opt.Ignore());
            CreateMap<UpdateUserCommand, User>()
                .ForMember(dest => dest.UserPermissions, opt => opt.Ignore());
        }
    }
}
