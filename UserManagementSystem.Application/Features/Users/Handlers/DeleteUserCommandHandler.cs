using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.Features.Users.Commands;
using UserManagementSystem.Domain.IRepositories;

namespace UserManagementSystem.Application.Features.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            await _unitOfWork.Users.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
