using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(
            IDiscountRepository discountRepository, 
            IMapper mapper,
            ILogger<DiscountService> logger)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var discount = await _discountRepository.GetDiscount(request.ProductName);

            if (discount is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name '{request.ProductName}' not found."));
            }

            _logger.LogInformation("Discount is retrieved for product with name '{productName}' and amount equal to {amount}", discount.ProductName, discount.Amount);

            return _mapper.Map<CouponModel>(discount);
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var discount = _mapper.Map<Coupon>(request.Coupon);

            await _discountRepository.CreateDiscount(discount);

            _logger.LogInformation("Discount is successfully created for the product with name '{productName}'", discount.ProductName);

            return _mapper.Map<CouponModel>(discount);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var discount = _mapper.Map<Coupon>(request.Coupon);

            await _discountRepository.UpdateDiscount(discount);

            _logger.LogInformation("Discount is successfully updated for the product with name '{productName}'", discount.ProductName);

            return _mapper.Map<CouponModel>(discount);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.DeleteDiscount(request.ProductName);

            _logger.LogInformation("Discount is successfully deleted for the product with name '{productName}'", request.ProductName);

            return new DeleteDiscountResponse
            {
                Success = deleted
            };
        }
    }
}
