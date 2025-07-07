namespace TaskMasterTutorial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DueDate = c.DateTime(),
                        Name = c.String(),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "StatusId", "dbo.Status");
            DropIndex("dbo.Tasks", new[] { "StatusId" });
            DropTable("dbo.Tasks");
        }
    }
}
