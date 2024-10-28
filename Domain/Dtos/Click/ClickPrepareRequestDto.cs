namespace Domain.Dtos.Click;

public class ClickPrepareRequestDto
{
    public long click_trans_id { get; set; } // CLICK tizimida tranzaksiya ID (bigint)
    public int service_id { get; set; } // Xizmat ID
    public long click_paydoc_id { get; set; } // CLICK tizimida to'lov hujjati ID (bigint)
    public string merchant_trans_id { get; set; } // Yetkazib beruvchining tranzaksiya ID / hisob raqami
    public float amount { get; set; } // To'lov summasi
    public int action { get; set; } // Amal turi (masalan, to'lovni tasdiqlash yoki qaytarish)
    public int error { get; set; } // Xato kodi
    public string sign_time { get; set; } // Imzo yaratilgan vaqt (string)
    public string error_note { get; set; } // Xato haqida izoh
    public string sign_string { get; set; } // Imzo qatori
}
