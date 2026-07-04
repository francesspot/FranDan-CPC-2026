namespace FranDanBackend
{
    public class MySeeder
    {
        private readonly MyContext _context;

        public MySeeder(MyContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
        }
    }
}
