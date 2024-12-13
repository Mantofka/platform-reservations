using Application.Abstractions.Tenant;
using Application.Contracts;
using Application.Contracts.Room;
using Application.Contracts.Tenant;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence.Abstractions;

namespace Infrastructure.Services.Tenants;

public class TenantService : ITenantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<TenantCreateDto> _validator;

    public TenantService(IUnitOfWork unitOfWork, IValidator<TenantCreateDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<TenantResponseDto[]> GetPagedList()
    {
        var repository = _unitOfWork.GetTenants();
        var entities = await repository.GetPagedList().ConfigureAwait(false);
        
        var result = entities.Select(ToContract).ToArray();

        return result;
    }
    
    private static TenantResponseDto ToContract(Tenant tenant)
    {
        var rooms = tenant.Rooms != null
            ? tenant.Rooms.Select(room => new RoomResponseDto
            {
                Id = room.Id,
                Number = room.Number,
                Description = room.Description,
                Size = room.Size,
                FloorNumber = room.FloorNumber,
                Price = room.Price,
            }).ToList()
            : [];
        var result = new TenantResponseDto
        {
            Id = tenant.Id,
            Name = tenant.User.Name,
            Surname = tenant.User.Surname,
            BirthDate = tenant.User.DateOfBirth,
            PhoneNumber = tenant.User.PhoneNumber!,
            Email = tenant.User.Email!,
            Rooms = rooms,
            
        };

        return result;
    }
    
    public async Task<TenantResponseDto> Create(TenantCreateDto input)
    {
        var repository = _unitOfWork.GetTenants();
        var entity = await ToEntity(input);
        var validationResult = await _validator.ValidateAsync(input);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var result = await repository.CreateAsync(entity).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);

        var resultContract = ToContract(result);

        return resultContract;
    }
    
    public async Task<bool> Remove(Guid id)
    {
        var repository = _unitOfWork.GetTenants();
        await repository.RemoveAsync(id).ConfigureAwait(false);

        await _unitOfWork.Commit().ConfigureAwait(false);

        return true;
    }
    
    public async Task<TenantResponseDto?> FindById(Guid id)
    {
        var repository = _unitOfWork.GetTenants();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        var result = ToContract(entity);

        return result;
    }
    
    private async Task<Tenant> ToEntity(TenantCreateDto tenant)
    {
        var result = new Tenant
        {
            Id = tenant.Id ?? Guid.NewGuid(),
            UserId = tenant.UserId,
        };

        return result;
    }
    
    public async Task<TenantResponseDto?> Edit(Guid id, TenantUpdateDto tenant)
    {
        var repository = _unitOfWork.GetTenants();
        var entity = await repository.GetByIdAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            return null;
        }

        entity.User.Name = tenant.Name;
        entity.User.Surname = tenant.Surname;
        entity.User.PhoneNumber = tenant.PhoneNumber;
        entity.User.Email = tenant.Email;
        entity.User.DateOfBirth = tenant.BirthDate;

        await _unitOfWork.Commit().ConfigureAwait(false);
        
        var result = ToContract(entity);

        return result;
    }

    public async Task<TenantResponseDto> GetCurrentTenant(Guid userId)
    {
        var repository = _unitOfWork.GetTenants();
        var tenant = await repository.GetByUserIdAsync(userId);
        if (tenant == null)
        {
                
            var tenantEntity = new Tenant
            {
                Id = Guid.NewGuid(),
                UserId = userId,
            };
            
            await _unitOfWork.Commit().ConfigureAwait(false);
            return ToContract(tenantEntity);
        }
        return ToContract(tenant);
    }
    
}