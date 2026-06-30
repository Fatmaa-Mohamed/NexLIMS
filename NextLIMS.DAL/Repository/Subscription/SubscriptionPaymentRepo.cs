using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.Subscription
{
    public class SubscriptionPaymentRepo
    {
        private readonly ApplicationDbContext _db;

        public SubscriptionPaymentRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<SubscriptionPaymentIntent> CreateAsync(SubscriptionPaymentIntent intent)
        {
            await _db.SubscriptionPaymentIntents.AddAsync(intent);
            await _db.SaveChangesAsync();
            return intent;
        }

        public async Task<SubscriptionPaymentIntent?> GetByInvoiceIdAsync(string invoiceId)
        {
            return await _db.SubscriptionPaymentIntents
                .FirstOrDefaultAsync(x => x.InvoiceId == invoiceId);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
