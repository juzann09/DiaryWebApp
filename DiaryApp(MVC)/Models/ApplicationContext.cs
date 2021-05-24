using Microsoft.EntityFrameworkCore;

namespace DiaryApp_MVC_.Models
{
    public class ApplicationContext : DbContext
    {
        // таблица Встречи
        public DbSet<Meeting> Meetings { get; set; }
        // таблица Памятки
        public DbSet<Memo> Memos { get; set; }
        // таблица Дела
        public DbSet<ThingToDo> ThingsToDo { get; set; }
    
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}