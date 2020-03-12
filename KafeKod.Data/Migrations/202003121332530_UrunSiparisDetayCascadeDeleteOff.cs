namespace KafeKod.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrunSiparisDetayCascadeDeleteOff : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Uruns", newName: "Urunler");
            DropForeignKey("dbo.SiparisDetaylar", "UrunId", "dbo.Uruns");
            AddForeignKey("dbo.SiparisDetaylar", "UrunId", "dbo.Urunler", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiparisDetaylar", "UrunId", "dbo.Urunler");
            AddForeignKey("dbo.SiparisDetaylar", "UrunId", "dbo.Uruns", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Urunler", newName: "Uruns");
        }
    }
}
