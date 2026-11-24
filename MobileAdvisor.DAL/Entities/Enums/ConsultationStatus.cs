namespace MobileAdvisor.DAL.Entities.Enums;

public enum ConsultationStatus
{
    PendingExpertReview = 1,      // أرسل الطلب وينتظر مراجعة الخبير
    AwaitingCustomerApproval = 2, // قبل الخبير الطلب وينتظر موافقة العميل على السعر
    AwaitingPayment = 3,          // وافق العميل على السعر وينتظر الدفع التجريبي
    ReadyToStart = 4,             // تم تأكيد الدفع وأصبح الطلب جاهزًا للبدء
    InProgress = 5,               // بدأت الاستشارة النصية
    Completed = 6,                // انتهت الاستشارة وأصبح التقييم متاحًا
    Rejected = 7,                 // رفض الخبير الطلب
    Cancelled = 8,                // أُلغي الطلب
    PaymentFailed = 9             // فشل الدفع ولا يمكن بدء الاستشارة
}
