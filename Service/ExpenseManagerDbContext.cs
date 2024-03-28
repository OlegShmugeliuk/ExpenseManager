using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ExpenseManagerDbContext : DbContext
    {
        public DbSet<InfoBody> ExpensesDb { get; set; }
        public DbSet<IncomePerson> IncomesDb { get; set; } 

        public ExpenseManagerDbContext(DbContextOptions<ExpenseManagerDbContext> options) : base(options)
        {

        }
    }
}

