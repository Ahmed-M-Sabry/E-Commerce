using ECommerce.Application.Comman;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);

                if (!IsRequestAllowed(context))
                {
                    var response = ApiResponse<object>.Fail(
                        "Too many requests. Please try again later.",
                        HttpStatusCode.TooManyRequests
                    );

                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                var message = _environment.IsDevelopment()
                    ? ex.Message
                    : "Internal Server Error";

                var errors = _environment.IsDevelopment()
                    ? new List<string> { ex.StackTrace ?? "No stack trace available" }
                    : null;

                var response = ApiResponse<object>.Fail(
                    message,
                    HttpStatusCode.InternalServerError,
                    errors
                );

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    WriteIndented = true 
                });

                await context.Response.WriteAsync(json);
            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var cacheKey = $"Rate:{ip}";
            var now = DateTime.Now;

            var (timestamp, count) = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timestamp: now, count: 0);
            });

            if (now - timestamp < _rateLimitWindow)
            {
                if (count >= 80)
                    return false;

                _memoryCache.Set(cacheKey, (timestamp, count + 1), _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cacheKey, (now, 1), _rateLimitWindow);
            }

            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self';";
        }
    }
}


/*
using ECommerce.Application.Comman;                // استيراد الكلاس ApiResponse<T> عشان نستخدمه في شكل الريسبونس الموحد
using Microsoft.Extensions.Caching.Memory;         // لاستخدام الـ MemoryCache في عمل rate limiting (تحديد عدد الطلبات)
using System.Net;                                  // لاستخدام أكواد الـ HTTP مثل 500 و 429
using System.Text.Json;                            // لتحويل الريسبونس إلى JSON بشكل يدوي

namespace ECommerce.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;            // يمثل الميدل وير اللي بعد الميدل وير ده في الـ pipeline
        private readonly IHostEnvironment _environment;    // عشان نعرف هل التطبيق في Development ولا Production
        private readonly IMemoryCache _memoryCache;        // الكاش المؤقت في الذاكرة لتخزين عدد الطلبات لكل IP
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30); // النافذة الزمنية المسموح فيها بعدد معين من الطلبات (30 ثانية)

        // 🔧 Constructor: بيحقن الـ Dependencies المطلوبة
        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        // 🚀 النقطة الأساسية اللي بيبدأ منها التنفيذ في كل request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);  // 🛡️ تطبيق بعض الهيدرز الأمنية في كل Response

                // ⚙️ التحقق من إن الطلب مش تعدّى الحد المسموح به (rate limit)
                if (!IsRequestAllowed(context))
                {
                    // لو الطلبات كثيرة جدًا → نرجع 429 Too Many Requests
                    var response = ApiResponse<object>.Fail(
                        "Too many requests. Please try again later.",
                        HttpStatusCode.TooManyRequests
                    );

                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests; // كود الحالة 429
                    context.Response.ContentType = "application/json";                 // نوع المحتوى JSON
                    await context.Response.WriteAsJsonAsync(response);                 // إرسال الريسبونس كـ JSON
                    return;                                                            // وقف هنا ومتكملش الباقي
                }

                // ✅ لو كله تمام، نكمّل للميدل وير اللي بعدنا
                await _next(context);
            }
            catch (Exception ex)
            {
                // 💥 هنا بيتعامل مع أي Exception ما تمش التعامل معاه في مكان تاني
                var message = _environment.IsDevelopment()
                    ? ex.Message                  // لو في وضع Development → نعرض رسالة الخطأ الحقيقية
                    : "Internal Server Error";     // لو Production → نعرض رسالة عامة بس

                var errors = _environment.IsDevelopment()
                    ? new List<string> { ex.StackTrace ?? "No stack trace available" } // عرض تفاصيل الخطأ للمطور
                    : null;                                                           // إخفائها في الإنتاج

                // 🔙 تجهيز الرد النهائي باستخدام ApiResponse الموحد
                var response = ApiResponse<object>.Fail(
                    message,
                    HttpStatusCode.InternalServerError,
                    errors
                );

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // كود الحالة 500
                context.Response.ContentType = "application/json";                     // نوع المحتوى JSON

                // 🎨 تهيئة خيارات JSON لعرض منسق وواضح
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    WriteIndented = true // ✅ يجعل الـ JSON منسق ومقروء
                });

                await context.Response.WriteAsync(json); // إرسال الريسبونس النهائي
            }
        }

        // ⛔ دالة للتحقق هل المستخدم لسه مسموح له يعمل طلب ولا تجاوز الحد
        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown"; // جلب عنوان IP للعميل
            var cacheKey = $"Rate:{ip}";                                          // مفتاح مميز في الكاش لكل IP
            var now = DateTime.Now;                                               // الوقت الحالي

            // 🧠 نحاول نقرأ من الكاش (أو ننشئ قيمة جديدة لو مش موجود)
            var (timestamp, count) = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow; // مدة صلاحية الكاش (30 ثانية)
                return (timestamp: now, count: 0);                         // أول مرة نعمل entry جديدة
            });

            // ⏰ لو الطلبات في نفس الـ window الزمنية
            if (now - timestamp < _rateLimitWindow)
            {
                if (count >= 80)                // لو المستخدم عمل أكثر من 80 طلب خلال 30 ثانية
                    return false;               // 🚫 مش مسموح بطلب جديد

                _memoryCache.Set(cacheKey, (timestamp, count + 1), _rateLimitWindow); // تحديث عدد الطلبات
            }
            else
            {
                // 🔄 إعادة تعيين العداد لو المدة انتهت
                _memoryCache.Set(cacheKey, (now, 1), _rateLimitWindow);
            }

            return true; // ✅ الطلب مسموح به
        }

        // 🛡️ إضافة هيدرز أمان لمنع هجمات شائعة (XSS, Clickjacking, إلخ)
        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";             // يمنع المتصفح من تخمين نوع المحتوى
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";             // حماية من هجمات XSS
            context.Response.Headers["X-Frame-Options"] = "DENY";                       // منع تحميل الموقع داخل iframe (ضد clickjacking)
            context.Response.Headers["Referrer-Policy"] = "no-referrer";                // منع إرسال رابط الصفحة كـ Referrer
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self';";// تحديد المصادر الموثوقة للمحتوى (حماية متقدمة)
        }
    }
}



 */