
# Click.uz Integration

`Click.uz Integration` — bu **Click.uz** to'lov tizimi bilan integratsiya qilish uchun yaratilgan **ASP.NET Core Web API** loyihasi. Loyiha **.NET 8** da yozilgan

 

## Click Sozlamalari

Loyihada Click.uz uchun sozlamalar quyidagicha ko'rinishda bo'ladi:

```json
"ClickPayApi": {
  "Service_ID": "SERVICE_ID",
  "Merchant_ID": "MERCHANT_ID",
  "Secret_Key": "SECRET_KEY",
  "Merchant_User_ID": "MERCHANT_USER_ID",
  "Url": "https://my.click.uz/services/pay",
  "ReturnUrl": "RETURN_URL"
}
```

- **SERVICE_ID**: Click tizimida yaratilgan xizmat identifikatori (Service ID).
- **MERCHANT_ID**: Click tizimida yaratilgan savdogar (merchant) identifikatori.
- **SECRET_KEY**: Maxfiy kalit (to'lov xavfsizligini ta'minlash uchun ishlatiladi).
- **MERCHANT_USER_ID**: Click tizimida yaratilgan savdogar foydalanuvchi identifikatori.
- **Url**: To'lov uchun ishlatiladigan API URL.
- **ReturnUrl**: To'lov muvaffaqiyatli yoki muvaffaqiyatsiz bo'lgandan so'ng foydalanuvchini qaytaradigan URL.

## To'lov uchun URL olish

To'lov yaratish uchun quyidagi endpointdan foydalaniladi:

```
GET /ClickPay/CreatePayment
```

Bu endpoint to'lov jarayonini boshlash uchun kerakli URL manzilini qaytaradi.

## Click Uchun API Endpoints

Click.uz to'lov tizimi bilan ishlash uchun quyidagi API endpointlar mavjud:

- **POST /ClickPay/Prepare**:  
  To'lovni tayyorlash uchun endpoint. Bu so'rov orqali to'lov ma'lumotlari oldindan tayyorlanadi va tizimga yuboriladi.

- **POST /ClickPay/Complete**:  
  To'lovni yakunlash uchun endpoint. Ushbu so'rov orqali tayyorlangan to'lov tasdiqlanadi va yakunlanadi.

### CORS siyosati va IP-manzillar

Quyida keltirilgan IP manzillar uchun CORS siyosati o'rnatilgan. Bu manzillardan keladigan **POST** metodli so'rovlar qabul qilinadi va har qanday `header` qabul qilinadi:

```csharp
options.AddPolicy("Click", builder =>
{
    builder.WithOrigins(
        "http://213.230.65.140",
        "http://217.29.119.130",
        "http://217.29.119.131",
        "http://217.29.119.132",
        "http://217.29.119.133"
    )
    .WithMethods("POST")
    .AllowAnyHeader();
});
```
