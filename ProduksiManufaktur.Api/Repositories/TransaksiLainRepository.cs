namespace ProduksiManufaktur.Api.Repositories
{
    public interface ITransaksiLainRepository
    {
        Task<List<TransaksiLain>> Get();

        Task<TransaksiLain> Find(int id);

        Task<List<TransaksiLain>> Creates(List<TransaksiLain> transaksiLain);

        Task<TransaksiLain> Update(TransaksiLain transaksiLain);

        Task Delete(int id);
    }

    public class TransaksiLainRepository : ITransaksiLainRepository
    {
        private readonly AppDbContext _appDbContext;

        public TransaksiLainRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<TransaksiLain>> Get()
        {
            return await _appDbContext.TransaksiLain.OrderByDescending(x => x.Tanggal).ToListAsync();
        }

        public async Task<TransaksiLain> Find(int id)
        {
            return await _appDbContext.TransaksiLain.FirstAsync(x => x.Id == id);
        }

        public async Task<List<TransaksiLain>> Creates(List<TransaksiLain> transaksiLain)
        {
            for (int i = 0; i < transaksiLain.Count; i++)
                transaksiLain[i].Tanggal = (DateTime)(transaksiLain[i].InputTanggal + transaksiLain[i].InputWaktu)!;

            await _appDbContext.TransaksiLain.AddRangeAsync(transaksiLain);
            await _appDbContext.SaveChangesAsync();

            return transaksiLain;
        }

        public async Task<TransaksiLain> Update(TransaksiLain transaksiLain)
        {
            var rowsAffected = await _appDbContext.TransaksiLain.Where(x => x.Id == transaksiLain.Id).ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Tanggal, (DateTime)(transaksiLain.InputTanggal + transaksiLain.InputWaktu)!)
                .SetProperty(z => z.Jenis, transaksiLain.Jenis)
                .SetProperty(z => z.Kategori, transaksiLain.Kategori)
                .SetProperty(z => z.Nominal, transaksiLain.Nominal)
                .SetProperty(z => z.Keterangan, transaksiLain.Keterangan));

            if (rowsAffected == 0) throw new DbUpdateConcurrencyException();

            return transaksiLain;
        }

        public async Task Delete(int id)
        {
            await _appDbContext.TransaksiLain.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}