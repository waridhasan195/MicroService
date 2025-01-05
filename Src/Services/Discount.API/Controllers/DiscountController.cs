using CoreApiResponse;
using Discount.API.Models;
using Discount.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountController : BaseController
    {
        private readonly ICouponRepository couponRepository;

        public DiscountController( ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult>GetDiscount(string productId)
        {
            try
            {
                var coupon = await couponRepository.GetDiscount(productId);
                return CustomResult(coupon);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, System.Net.HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof (Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                var IsCreate = await couponRepository.CreateDiscount(coupon);
                if (IsCreate)
                {
                    return CustomResult("Coupon Created", coupon, HttpStatusCode.Created);
                }
                return  CustomResult("Coupon Create Failed",coupon, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            try
            {
                var IsCreate = await couponRepository.UpdateDiscount(coupon);
                if (IsCreate)
                {
                    return CustomResult("Coupon Updated", coupon, HttpStatusCode.Created);
                }
                return CustomResult("Coupon Update Failed", coupon, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string productId)
        {
            try
            {
                var IsDeleted = await couponRepository.DeleteDiscount(productId);
                if (IsDeleted)
                {
                    return CustomResult("Coupon Deleted", HttpStatusCode.OK);
                }
                return CustomResult("Coupon Deleted Failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
