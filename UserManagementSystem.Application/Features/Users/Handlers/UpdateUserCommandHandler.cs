using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.IRepositories;

namespace UserManagementSystem.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            _mapper.Map(request, user);

            user.UserPermissions.Clear();
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

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
