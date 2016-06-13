using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using VideoOnline.Data.UnitOfWork.Data.Core;
using VideoOnline.Models;

namespace VideoOnline.Data.EntityFrameWork.Data.Core
{
    public class EFContext : DbContext,IUnitOfWork
    {
        public EFContext() : base("ConnextionString")
        {
            Database.SetInitializer<EFContext>(new MigrateDatabaseToLatestVersion<EFContext, EFDbMigrationsConfiguration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public void Save()
        {
            try
            {
                this.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }

        public bool IsNotSubmit { get; set; }
    }

    internal sealed class EFDbMigrationsConfiguration : DbMigrationsConfiguration<EFContext>
    {
        public EFDbMigrationsConfiguration()
        {
            //任何 model class 的修改将会自动更新数据库
            AutomaticMigrationsEnabled = true;
            //可以接受自动迁移期间的数据丢失的值
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
