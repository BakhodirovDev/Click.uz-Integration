using Application.Interface.Repository.Transaction;
using Application.Interface.User;
using Application.Interfaces.AutoPay.Click;
using Domain.Dtos.Click;
using Domain.Entities.Transactions;
using Domain.Enums;
using Domain.Settings;
using Domen.DTO;
using Infrastructure.Context;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.AutoPay.Click
{
    public class ClickService : IClickService
    {
        //
        private readonly ApplicationDbContext _dbcontext;
        private readonly ClickSettings _clickSettings;
        private readonly IUserService _userService;
        private readonly ITransactionRepository _transactionRepository;

        public ClickService(IOptions<ClickSettings> clickSettings, ApplicationDbContext applicationDbContext, IUserService userService,
                            ITransactionRepository transactionRepository)
        {
            _clickSettings = clickSettings.Value;
            _dbcontext = applicationDbContext;
            _userService = userService;
            _transactionRepository = transactionRepository;
        }

        public async Task<RequestResponseDto<string>> Create(float amount, Guid userId)
        {
            string transactionId = await _transactionRepository.GenerateTransactionIdAsync();

            var user = await _dbcontext.Users.FindAsync(userId);
            if (user == null)
            {
                return new RequestResponseDto<string>
                {
                    Success = false,
                    Message = "Foydalanuvchi topilmadi",
                    Data = null
                };
            }

            var url = $"{_clickSettings.Url}?service_id={_clickSettings.Service_ID}&merchant_id={_clickSettings.Merchant_ID}&amount={amount}&transaction_param={transactionId}&return_url={_clickSettings.ReturnUrl}";

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TransactionId = transactionId,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Status = TransactionStatus.Pending,
                PaymentType = PaymentType.Click
            };

            await _transactionRepository.SaveChangesAsync(transaction);

            return new RequestResponseDto<string>
            {
                Success = true,
                Message = "Havola yaratildi",
                Data = url

            }; ;
        }

        public async Task<ClickPrepareResponseDto> Prepare(ClickPrepareRequestDto clickRequest)
        {
            string calculatedSignString = await GenerateSignString_Prepare(
                clickRequest.click_trans_id,
                clickRequest.service_id,
                clickRequest.merchant_trans_id,
                clickRequest.amount,
                clickRequest.action,
                clickRequest.sign_time
            );

            if (!calculatedSignString.Equals(clickRequest.sign_string, StringComparison.OrdinalIgnoreCase))
            {
                return new ClickPrepareResponseDto
                {
                    click_trans_id = clickRequest.click_trans_id,
                    merchant_trans_id = clickRequest.merchant_trans_id,
                    merchant_prepare_id = 0,
                    error = 1,
                    error_note = "Invalid sign_string"
                };
            }

            var dbTransaction = await _transactionRepository.GetTransactionByTransactionIdAsync(clickRequest.merchant_trans_id);

            if (dbTransaction == null)
            {
                return new ClickPrepareResponseDto
                {
                    click_trans_id = clickRequest.click_trans_id,
                    merchant_trans_id = clickRequest.merchant_trans_id,
                    merchant_prepare_id = 0,
                    error = 1,
                    error_note = "Transaction not found"
                };
            }

            dbTransaction.LastUpdatedAt = DateTime.UtcNow;  // Optionally update LastUpdatedAt
            dbTransaction.PaymentTransId = clickRequest.click_trans_id.ToString(); 

            await _transactionRepository.UpdateChangesAsync(dbTransaction);

            var result = new ClickPrepareResponseDto
            {
                click_trans_id = clickRequest.click_trans_id,  // Ensure it's BigInteger or string if needed
                merchant_trans_id = clickRequest.merchant_trans_id,
                merchant_prepare_id = dbTransaction.TransactionId.Length,  // Based on the actual TransactionId field
                error = 0, // Success
                error_note = "Success"
            };

            return result;
        }

        public async Task<ClickCompleteResponseDto> Complete(ClickCompleteRequestDto clickRequest)
        {
            string calculatedSignString = await GenerateSignString_Complete(
                clickRequest.click_trans_id,
                clickRequest.service_id,
                clickRequest.click_paydoc_id,
                clickRequest.merchant_trans_id,
                clickRequest.merchant_prepare_id,
                clickRequest.amount,
                clickRequest.action,
                clickRequest.sign_time
            );
            
            if (!calculatedSignString.Equals(clickRequest.sign_string, StringComparison.OrdinalIgnoreCase))
            {
                return new ClickCompleteResponseDto
                {
                    click_trans_id = clickRequest.click_trans_id,
                    merchant_trans_id = clickRequest.merchant_trans_id,
                    error = 1,
                    error_note = "Invalid sign_string"
                };
            }

            var dbTransaction = await _transactionRepository.GetTransactionByTransactionIdAsync(clickRequest.merchant_trans_id);

            if (dbTransaction == null)
            {
                return new ClickCompleteResponseDto
                {
                    click_trans_id = clickRequest.click_trans_id,
                    merchant_trans_id = clickRequest.merchant_trans_id,
                    error = 1,
                    error_note = "Transaction not found"
                };
            }

            if (clickRequest.error == 0)
            {
                var user = await _userService.GetUserByIdAsync(dbTransaction.UserId);

                user.Balance += dbTransaction.Amount;

                dbTransaction.Status = TransactionStatus.Completed; // Mark as successfully completed
            }
            else
            {
                dbTransaction.Status = TransactionStatus.Failed; // Mark as failed if there was an error
            }

            dbTransaction.LastUpdatedAt = DateTime.UtcNow;

            await _transactionRepository.UpdateChangesAsync(dbTransaction);

            var result = new ClickCompleteResponseDto
            {
                click_trans_id = clickRequest.click_trans_id,
                merchant_trans_id = clickRequest.merchant_trans_id,
                error = clickRequest.error,
                error_note = clickRequest.error_note
            };

            return result;
        }
        
        public async Task<string> GenerateSignString_Complete(BigInteger click_trans_id, int service_id, long click_paydoc_id, string merchant_trans_id, int merchant_prepare_id, float amount, int action, string sign_time)
        {
            string secretKey = _clickSettings.Secret_Key;
            string signString = $"{click_trans_id}{service_id}{secretKey}{merchant_trans_id}{merchant_prepare_id}{amount}{action}{sign_time}";

            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(signString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var sb = new System.Text.StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return await Task.FromResult(sb.ToString());
            }
        }

        public async Task<string> GenerateSignString_Prepare(BigInteger clickTransId, int serviceId, string merchantTransId, float amount, int action, string signTime)
        {
            string secretKey = _clickSettings.Secret_Key;
            string concatenatedString = $"{clickTransId}{serviceId}{secretKey}{merchantTransId}{amount}{action}{signTime}";

            // MD5 hashini hisoblash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(concatenatedString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Hashni heksadecimal formatga o'tkazish
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return await Task.FromResult(sb.ToString());
            }
        }
    }
}
