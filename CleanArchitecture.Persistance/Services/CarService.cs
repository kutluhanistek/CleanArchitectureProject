using AutoMapper;
using CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;
using CleanArchitecture.Application.Features.CarFeatures.Queries.GettAllCar;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistance.Context;
using EntityFrameworkCorePagination.Nuget.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistance.Services;

public sealed class CarService : ICarService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public CarService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateAsync(CreateCarCommand request, CancellationToken cancellationToken)
    {
        Car car = _mapper.Map<Car>(request);
        await _context.Set<Car>().AddAsync(car, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

    }

    public async Task<PaginationResult<Car>> GetAllAsync(GetAllCarQuery request, CancellationToken cancellationToken)
    {
        PaginationResult<Car> cars = 
            await _context
            .Set<Car>().Where(p => p.Name.ToLower().Contains(request.Search.ToLower()))
            .OrderBy(p => p.EnginePower)
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);//pagination yapısı ile bir sayfada kaç eleman gösterileceğini ayarladık
        return cars;
      
    }
}
