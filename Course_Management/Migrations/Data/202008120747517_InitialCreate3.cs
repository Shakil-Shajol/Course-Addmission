namespace Course_Management.Migrations.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainees", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainees", "UserName");
        }
    }
}
