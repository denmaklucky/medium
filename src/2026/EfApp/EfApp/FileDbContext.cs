using Microsoft.EntityFrameworkCore;

namespace EfApp;

public class FileDbContext : DbContext
{
    public DbSet<File> Files { get; set; }
    
    public DbSet<FileAttribute> FileAttributes { get; set; }
}