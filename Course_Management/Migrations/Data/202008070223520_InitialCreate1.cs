namespace Course_Management.Migrations.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Duration", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "Quizes", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "Assesments", c => c.Int(nullable: false));
            DropColumn("dbo.Courses", "CourseHour");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "CourseHour", c => c.Int(nullable: false));
            DropColumn("dbo.Courses", "Assesments");
            DropColumn("dbo.Courses", "Quizes");
            DropColumn("dbo.Courses", "Duration");
        }
    }
}
