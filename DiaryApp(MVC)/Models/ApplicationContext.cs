using Microsoft.EntityFrameworkCore;

namespace DiaryApp_MVC_.Models
{
    public class ApplicationContext : DbContext
    {
        // ������� �������
        public DbSet<Meeting> Meetings { get; set; }
        // ������� �������
        public DbSet<Memo> Memos { get; set; }
        // ������� ����
        public DbSet<ThingToDo> ThingsToDo { get; set; }
    
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}