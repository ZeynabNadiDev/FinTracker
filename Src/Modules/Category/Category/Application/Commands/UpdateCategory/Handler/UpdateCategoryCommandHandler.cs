using Category.Application.DTO;
using Category.Domain.Repository;
using Category.Domain.Uow;
using FinTracker.SharedKernel.Results;
using MediatR;



namespace Category.Application.Commands.UpdateCategory.Handler

{
    public sealed class UpdateCategoryCommandHandler
        : IRequestHandler<UpdateCategoryCommand, Result<CategoryResponseDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryResponseDto>> Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

            if (category is null)
            {
                return Result<CategoryResponseDto>.Failure("Category.NotFound: The specified category was not found.");
            }
        

            if (category.UserId != request.UserId)
            {
                return Result<CategoryResponseDto>.Failure("Category.Forbidden: You do not have permission to update this category.");
            }

            category.Update(request.Name, request.Description, request.Type);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new CategoryResponseDto(
                category.Id,
                category.Name,
                category.Description,
                category.UserId,
                category.Type
            );

            return Result<CategoryResponseDto>.Success(response);
        }
    }
}