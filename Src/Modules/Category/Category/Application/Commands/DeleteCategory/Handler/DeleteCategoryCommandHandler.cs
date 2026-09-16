using Category.Domain.Repository;
using Category.Domain.Uow;
using FinTracker.SharedKernel.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Category.Application.Commands.DeleteCategory
{
public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Retrieve entity from database
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure("Category.NotFound: The specified category was not found.");
        }

        // 2. Security check: Ensure the category belongs to the requesting user
        if (category.UserId != request.UserId)
        {
            return Result.Failure("Category.Forbidden: You do not have permission to delete this category.");
        }

        // 3. Execute domain logic (Soft Delete and Domain Event triggering)
        category.Remove();

        // 4. Persist changes to the database
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
        }
    }
}
