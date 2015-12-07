namespace SistemaDeGestionDeFilas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hola : DbMigration
    {
        public override void Up()
        {
            AddColumn("crm.punto", "atact", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("crm.punto", "atact");
        }
    }
}
