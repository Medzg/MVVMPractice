namespace MVVM.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProgramingLang : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgrammingLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Friend", "FavoriteLangugeId", c => c.Int());
            CreateIndex("dbo.Friend", "FavoriteLangugeId");
            AddForeignKey("dbo.Friend", "FavoriteLangugeId", "dbo.ProgrammingLanguage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "FavoriteLangugeId", "dbo.ProgrammingLanguage");
            DropIndex("dbo.Friend", new[] { "FavoriteLangugeId" });
            DropColumn("dbo.Friend", "FavoriteLangugeId");
            DropTable("dbo.ProgrammingLanguage");
        }
    }
}
