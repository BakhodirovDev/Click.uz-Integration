using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Entities.Users;

namespace Domain.Entities.Transactions
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); // Tranzaksiya ID

        [ForeignKey("User")]
        public Guid UserId { get; set; }  // Foydalanuvchi bilan bog'liq bo'lgan ID

        [Required]
        public string TransactionId { get; set; }  // Tizim orqali yuborilgan tranzaksiya ID

        public string? PaymentTransId { get; set; }  // Click yoki boshqa to'lov tizimi orqali kelgan tranzaksiya ID

        [Required]
        public float Amount { get; set; } // Tranzaksiya summasi

        [Required]
        public DateTime CreatedAt { get; set; }  // Tranzaksiya yaratilgan vaqt

        public DateTime? LastUpdatedAt { get; set; } // Tranzaksiya oxirgi marta yangilangan vaqt

        [Required]
        public TransactionStatus Status { get; set; }  // Tranzaksiya holati (Pending, Completed, Failed, va hokazo)

        [Required]
        public PaymentType PaymentType { get; set; }  // To'lov turi (Click, Payme, UPay, va h.k.)

        // Navigatsiya xususiyati
        public User? User { get; set; }  // Tranzaksiyani amalga oshirgan foydalanuvchi
    }
}
