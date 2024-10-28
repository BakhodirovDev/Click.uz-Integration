namespace Domain.Dtos.Click;

public class ClickCompleteResponseDto
{
    public long click_trans_id { get; set; } // CLICK tizimida to'lov ID (bigint)
    public string merchant_trans_id { get; set; } // Buyurtma ID / shaxsiy hisob / yetkazib beruvchining billing tizimidagi login
    public int? merchant_confirm_id { get; set; } // Yetkazib beruvchining billing tizimidagi to'lov ID
    public int error { get; set; } // Xato kodi
    public string error_note { get; set; } // Xato haqida izoh
}
