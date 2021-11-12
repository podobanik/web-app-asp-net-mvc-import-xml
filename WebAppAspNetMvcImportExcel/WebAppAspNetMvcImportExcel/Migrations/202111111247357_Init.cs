namespace WebAppAspNetMvcJs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartImport = c.DateTime(nullable: false),
                        EndImport = c.DateTime(nullable: false),
                        SuccessCount = c.Int(nullable: false),
                        FailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogHistories");
        }
    }
}
