using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.Features.Users.Queries;
using UserManagementSystem.Application.IServices;
using UserManagementSystem.Domain.IRepositories;

namespace UserManagementSystem.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _permissionService = permissionService;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
            {
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Permissions = (await _permissionService.GetUserPermissionsAsync(user.Id)).ToList();

            return userDto;
        }
    }
}
