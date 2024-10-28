namespace Domain.Settings;

public class ClickSettings
{
    public int Service_ID { get; set; } // Xizmat ID (CLICK tizimida xizmat identifikatori)
    public int Merchant_ID { get; set; } // Savdogar (merchant) ID
    public string Secret_Key { get; set; } // Maxfiy kalit (to'lovni tasdiqlash uchun ishlatiladi)
    public string Merchant_User_ID { get; set; } // Savdogar (merchant) foydalanuvchi ID
    public string Url { get; set; } // CLICK API uchun URL
    public string ReturnUrl { get; set; } // To'lovdan so'ng qaytish URL manzili
}
