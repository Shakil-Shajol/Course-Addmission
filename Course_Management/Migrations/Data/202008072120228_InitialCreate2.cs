namespace Course_Management.Migrations.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trainees", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Trainers", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trainers", "Gender");
            DropColumn("dbo.Trainees", "Gender");
        }
    }
}
