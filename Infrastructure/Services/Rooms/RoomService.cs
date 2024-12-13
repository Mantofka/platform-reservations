using Application.Abstractions.Room;
using Application.Contracts;
using Application.Contracts.Room;
using Application.Contracts.Tenant;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.RoomTenant;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services.Rooms;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RoomCreateDto> _validator;
    private readonly IValidator<AssignTenantDto> _assignTenantValidator;
    public RoomService(IUnitOfWork unitOfWork, IValidator<RoomCreateDto> validator, IValidator<AssignTenantDto> assignTenantValidator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _assignTenantValidator = assignTenantValidator;
    }
    
    public async Task<RoomResponseDto[]> GetPagedList()
    {
        var repository = _unitOfWork.GetRooms();
        var entities = await repository.GetPagedList().ConfigureAwait(false);
        
        var result = entities.Select(ToContract).ToArray();

        return result;
    }
    
    public async Task<RoomResponseDto[]> GetPagedColivingRoomsList(Guid colivingId)
    {
        var repository = _unitOfWork.GetRooms();
        var entities = await repository.GetPagedColivingRoomList(colivingId).ConfigureAwait(false);
        
        var result = entities.Select(ToContract).ToArray();

        return result;
    }
    
    private static RoomResponseDto ToContract(Room room)
    {
        var result = new RoomResponseDto
        {
            Id = room.Id,
            Number = room.Number,
            Description = room.Description,
            Size = room.Size,
            FloorNumber = room.FloorNumber,
            Price = room.Price,
            Coliving = new ColivingResponseDto
            {
                Name = room.Coliving.Name,
                Address = room.Coliving.Address,
                Id = room.Coliving.Id,
                Email = room.Coliving.Email,
            },
            Tenants = room.Tenants?.Select(tenant => new TenantResponseDto
            {
                Name = tenant.User.Name,
                Surname = tenant.User.Surname,
                PhoneNumber = tenant.User.PhoneNumber,
                Email = tenant.User.Email,
                Id = tenant.Id,
                BirthDate = tenant.User.DateOfBirth
            }).ToList()
        };

        return result;
    }
    
    public async Task<RoomResponseDto> Create(RoomCreateDto input)
    {
        var repository = _unitOfWork.GetRooms();
        var entity = ToEntity(input);
        ValidationResult validationResult = await _validator.ValidateAsync(input);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var result = await repository.CreateAsync(entity).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);

        var resultContract = ToContract(result);

        return resultContract;
    }
    
    public async Task AssignTenant(AssignTenantDto input, Guid userId)
    {
        var repository = _unitOfWork.GetRooms();
        var tenantRepository = _unitOfWork.GetTenants();

        if (input.TenantId == null)
        {
            var tenant = await tenantRepository.GetByUserIdAsync(userId);
            if (tenant == null)
            {
                
                var tenantEntity = new Tenant
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                };

                tenant = await tenantRepository.CreateAsync(tenantEntity!).ConfigureAwait(false);
            }
            input.TenantId = tenant.Id;
        }
        var existingAssignment = await repository.IsTenantAssignedToRoomAsync(input.TenantId.Value, input.Roomid).ConfigureAwait(false);
        if (existingAssignment)
        { 
            throw new InvalidOperationException("The tenant is already assigned to this room.");
        }
        
        var roomTenant = new RoomTenant
        {
            RoomId = input.Roomid,
            TenantId = input.TenantId!.Value,
        };
        
        await repository.AssignTenantAsync(roomTenant).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);
    }
    
    public async Task<bool> Remove(Guid id)
    {
        var repository = _unitOfWork.GetRooms();
        await repository.RemoveAsync(id).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);

        return true;
    }
    
    public async Task<RoomResponseDto?> FindById(Guid id)
    {
        var repository = _unitOfWork.GetRooms();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        var result = ToContract(entity);

        return result;
    }
    
    private static Room ToEntity(RoomCreateDto room)
    {
        var result = new Room
        {
            Id = room.Id ?? Guid.NewGuid(),
            Number = room.Number,
            Description = room.Description,
            Size = room.Size,
            FloorNumber = room.FloorNumber,
            Price = room.Price,
            ColivingId = room.ColivingId
        };

        return result;
    }
    
    public async Task<RoomResponseDto?> Edit(Guid id, RoomCreateDto room)
    {
        var repository = _unitOfWork.GetRooms();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        entity.Number = room.Number;
        entity.Description = room.Description;
        entity.Size = room.Size;
        entity.FloorNumber = room.FloorNumber;
        entity.Price = room.Price;

        await _unitOfWork.Commit().ConfigureAwait(false);
        
        var result = ToContract(entity);

        return result;
    }
    
    public async Task<Guid?> GetOwnerIdByRoomId(Guid roomId)
    {
        var repository = _unitOfWork.GetRooms();

        var foundColivingId = await repository.GetOwnerIdByRoomId(roomId);

        return foundColivingId;
    }
}