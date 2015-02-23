namespace TNS.Importer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SystemFileNameWithExtension", c => c.String());
            AddColumn("dbo.Products", "CurrentProcessingFolder", c => c.String());
            DropColumn("dbo.Products", "SystemFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "SystemFileName", c => c.String());
            DropColumn("dbo.Products", "CurrentProcessingFolder");
            DropColumn("dbo.Products", "SystemFileNameWithExtension");
        }
    }
}
