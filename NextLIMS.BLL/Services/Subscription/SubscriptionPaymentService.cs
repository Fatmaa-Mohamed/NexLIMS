using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.Subscription;

namespace NextLIMS.BLL.Services.Subscription
{
    public class InitiatePaymentResult
    {
        public string InvoiceId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? PaymentUrl { get; set; }   // null in dev, real URL in prod
    }

    public class SubscriptionPaymentService
    {
        private readonly SubscriptionPaymentRepo _paymentRepo;
        private readonly SubscriptionUpdateRepo _updateRepo;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _http;
        private readonly IHostEnvironment _env;

        public SubscriptionPaymentService(
            SubscriptionPaymentRepo paymentRepo,
            SubscriptionUpdateRepo updateRepo,
            ApplicationDbContext db,
            IHttpContextAccessor http,
            IHostEnvironment env)
        {
            _paymentRepo = paymentRepo;
            _updateRepo  = updateRepo;
            _db          = db;
            _http        = http;
            _env         = env;
        }

        public int TenantId =>
            int.Parse(_http.HttpContext!.User.FindFirst("TenantId")!.Value);

        public async Task<decimal> CalculateAmountAsync(string action, int? sampleNumber, int? planId)
        {
            return action switch
            {
                "add_quota" => (sampleNumber ?? 0) * 25m,
                "renew"     => await GetCurrentPlanPriceAsync(TenantId),
                "upgrade"   => await GetPlanPriceAsync(planId!.Value),
                _           => throw new ArgumentException("Unknown action")
            };
        }

        public async Task<InitiatePaymentResult> CreatePendingIntentAsync(
            string action, int? sampleNumber, int? planId, decimal amount,
            string invoiceId, string? paymentUrl)
        {
            await _paymentRepo.CreateAsync(new SubscriptionPaymentIntent
            {
                TenantId     = TenantId,
                InvoiceId    = invoiceId,
                Action       = action,
                SampleNumber = sampleNumber,
                PlanId       = planId,
                Amount       = amount,
                Status       = "Pending",
                CreatedAt    = DateTime.UtcNow,
            });

            return new InitiatePaymentResult
            {
                InvoiceId  = invoiceId,
                Amount     = amount,
                PaymentUrl = paymentUrl,
            };
        }

        public async Task<string> ExecuteAsync(string invoiceId)
        {
            var intent = await _paymentRepo.GetByInvoiceIdAsync(invoiceId);

            if (intent == null)   return "Payment record not found.";
            if (intent.Status == "Paid")   return "Already processed.";
            if (intent.Status == "Failed") return "Payment failed.";

            switch (intent.Action)
            {
                case "add_quota":
                    await _updateRepo.AddQuota(intent.TenantId, intent.SampleNumber ?? 0);
                    break;
                case "renew":
                    await _updateRepo.UpgrdeSubscription(intent.TenantId, intent.PlanId ?? 0);
                    break;
                case "upgrade":
                    var tenant = await _db.Tenants.FindAsync(intent.TenantId);
                    var newPlan = await _db.SubscriptionPlans.FindAsync(intent.PlanId ?? 0);
                    bool isDowngrade = newPlan != null && tenant != null &&
                                       newPlan.MonthlySampleLimit < (tenant.MonthlySampleLimit ?? 0);
                    if (isDowngrade)
                        await _updateRepo.UpgrdeSubscription(intent.TenantId, intent.PlanId ?? 0);
                    else
                        await _updateRepo.UpgradeKeepRemainingAsync(intent.TenantId, intent.PlanId ?? 0);
                    break;
                default:
                    return "Unknown action.";
            }

            intent.Status = "Paid";
            await _paymentRepo.SaveChangesAsync();

            return intent.Action switch
            {
                "add_quota" => $"{intent.SampleNumber} samples added to your quota.",
                "renew"     => "Plan renewed for another month.",
                "upgrade"   => "Subscription upgraded successfully.",
                _           => "Done."
            };
        }

        public bool IsDev => _env.IsDevelopment();

        // ── Helpers ───────────────────────────────────────────────────────────

        private async Task<decimal> GetCurrentPlanPriceAsync(int tenantId)
        {
            var tenant = await _db.Tenants
                .Include(t => t.SubscriptionPlan)
                .FirstOrDefaultAsync(t => t.Id == tenantId);
            return tenant?.SubscriptionPlan?.Price ?? 0m;
        }

        private async Task<decimal> GetPlanPriceAsync(int planId)
        {
            var plan = await _db.SubscriptionPlans.FindAsync(planId);
            return plan?.Price ?? 0m;
        }

        public async Task<(string tenantName, string adminEmail, string adminName)> GetTenantInfoAsync()
        {
            var tenant = await _db.Tenants
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == TenantId);
            var admin = tenant?.Users.FirstOrDefault();
            return (tenant?.Name ?? "", admin?.Email ?? "", admin?.Name ?? "Admin");
        }
    }
}
