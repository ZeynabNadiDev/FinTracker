using Category.Application.DTO;
using Category.Domain.Entities.Category;
using Category.Domain.Repository;
using Category.Domain.Uow;
using FinTracker.SharedKernel.Results;
using MediatR;
using Category.Application.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Category.Application.Commands.CreateCategory.Handler;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryResponseDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryResponseDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.Domain.Entities.Category.Category.Create(
            request.Name,
            request.Description,
            request.UserId,
            request.Type);

        await _categoryRepository.AddAsync(category, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CategoryResponseDto(
            category.Id,
            category.Name,
            category.Description,
            category.UserId,
            category.Type);

        return Result<CategoryResponseDto>.Success(response);
    }
}
