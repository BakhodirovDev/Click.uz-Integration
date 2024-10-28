using Application.Interface.User;
using Application.Interfaces.AutoPay.Click;
using Domain.Dtos.Click;
using Domain.Dtos.Url;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Click.uz_Integration.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class ClickPayController : ControllerBase
    {
        private readonly IClickService _clickPaymentService;
        private readonly IUserService _userService;

        public ClickPayController(IClickService clickPaymentService, IUserService userService)
        {
            _clickPaymentService = clickPaymentService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<GetPaymentUrlDto>> CreatePayment(float amount, Guid UserId)
        {
            try
            {
                var userId = UserId;

                // UserId ni Bearer Token orqali o'qib olish ham mumkin

                /*var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = await _userService.GetUserIdFromTokenAsync(token);*/


                if (userId == null)
                {
                    //return Unauthorized("Invalid token");
                    return BadRequest("UserId noto'g'ti");
                }

                if (amount < 1000)
                {
                    return BadRequest("Miqdor Min 1000 bo'lishi kerak");
                }

                var url = await _clickPaymentService.Create(amount, userId);

                if (!url.Success)
                {
                    return BadRequest(url.Message);
                }

                var result = new GetPaymentUrlDto
                {
                    PayUrl = url.Data
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Havolani yaratishda xatolik yuz berdi. {ex}");
            }
        }

        [HttpPost]
        [EnableCors("Click")]
        public async Task<IActionResult> Prepare([FromForm] ClickPrepareRequestDto clickRequest)
        {
            try
            {
                var response = await _clickPaymentService.Prepare(clickRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ClickPrepareResponseDto
                {
                    click_trans_id = clickRequest.click_trans_id,
                    merchant_trans_id = clickRequest.merchant_trans_id,
                    merchant_prepare_id = 0,
                    error = 1,
                    error_note = ex.Message
                });
            }
        }

        [HttpPost]
        [EnableCors("Click")]
        public async Task<IActionResult> Complete([FromForm] ClickCompleteRequestDto clickRequest)
        {
            try
            {
                var response = await _clickPaymentService.Complete(clickRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ClickCompleteResponseDto
                {
                    click_trans_id = clickRequest.click_trans_id,
                    merchant_trans_id = clickRequest.merchant_trans_id,
                    error = 1,
                    error_note = ex.Message
                });
            }
        }

    }
}
