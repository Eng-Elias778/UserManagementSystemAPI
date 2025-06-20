using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCrypt.Net;
using System.Threading.Tasks;
using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.IRepositories;
using AutoMapper;

namespace UserManagementSystem.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            //var existingUser = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
            //if (existingUser != null)
            //{
            //    throw new Exception("Username already exists.");
            //}

            var user = _mapper.Map<User>(request);
            user.PasswordHash = request.Password;
            user.UserPermissions = new List<UserPermission>();

            if (request.Permissions != null && request.Permissions.Any())
            {
                foreach (var permissionName in request.Permissions)
                {
                    var permission = await _unitOfWork.Permissions.GetByNameAsync(permissionName);
                    if (permission == null)
                    {
                        throw new Exception($"Permission '{permissionName}' not found.");
                    }
                    user.UserPermissions.Add(new UserPermission { User = user, Permission = permission });
                }
            }

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
