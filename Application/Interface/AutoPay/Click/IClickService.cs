using Domain.Dtos.Click;
using Domen.DTO;
using System.Numerics;

namespace Application.Interfaces.AutoPay.Click;

public interface IClickService
{
    Task<ClickPrepareResponseDto> Prepare(ClickPrepareRequestDto clickRequest);
    Task<ClickCompleteResponseDto> Complete(ClickCompleteRequestDto clickRequest);
    Task<RequestResponseDto<string>> Create(float amount, Guid userId);
    Task<string> GenerateSignString_Prepare(BigInteger clickTransId, int serviceId, string merchantTransId, float amount, int action, string signTime);
    Task<string> GenerateSignString_Complete(BigInteger click_trans_id, int service_id, long click_paydoc_id,string merchant_trans_id, int merchant_prepare_id, float amount, int action, string sign_time);
}
