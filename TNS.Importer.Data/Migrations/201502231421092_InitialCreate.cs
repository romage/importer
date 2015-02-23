namespace TNS.Importer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        OriginalFileName = c.String(),
                        SystemFileName = c.String(),
                        ProcessState = c.Int(nullable: false),
                        Scorer = c.String(),
                        ScorerEmail = c.String(),
                        DateOfScoreInput = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScoreName = c.String(),
                        ScoreValue = c.Double(nullable: false),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "Product_Id", "dbo.Products");
            DropIndex("dbo.Scores", new[] { "Product_Id" });
            DropTable("dbo.Scores");
            DropTable("dbo.Products");
        }
    }
}
