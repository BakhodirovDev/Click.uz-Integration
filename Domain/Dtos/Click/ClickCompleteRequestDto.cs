namespace Domain.Dtos.Click;

public class ClickCompleteRequestDto
{
    public long click_trans_id { get; set; }
    public int service_id { get; set; }
    public long click_paydoc_id { get; set; }
    public string merchant_trans_id { get; set; }
    public int merchant_prepare_id { get; set; }
    public float amount { get; set; }
    public int action { get; set; }
    public int error { get; set; }
    public string error_note { get; set; }
    public string sign_time { get; set; }
    public string sign_string { get; set; }
}
