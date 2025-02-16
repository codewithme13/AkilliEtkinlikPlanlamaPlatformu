namespace Akıllı_Etkinlik_Planlama_Platformu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deneme01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kullanicilars", "IsAdmin", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kullanicilars", "IsAdmin");
        }
    }
}
