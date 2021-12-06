using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SchoolbookApp.Models;

namespace SchoolbookApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SchoolbookApp.Models.SchoolClass> SchoolClass { get; set; }
        public DbSet<SchoolbookApp.Models.Subject> Subject { get; set; }
        public DbSet<SchoolbookApp.Models.SubjectType> SubjectType { get; set; }
    }
}
