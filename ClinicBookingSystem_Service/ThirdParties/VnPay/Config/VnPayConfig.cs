namespace ClinicBookingSystem_Service.ThirdParties.VnPay.Config;

public class VnPayConfig
{
    public string vnp_Returnurl { get; set; }
    public string vnp_Url { get; set; }
    public string vnp_TmnCode { get; set; }
    public string vnp_HashSecret { get; set; }
    public string PaymentSuccess { get; set; }
    public string PaymentFail { get; set; }
}