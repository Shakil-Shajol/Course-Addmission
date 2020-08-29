namespace Course_Management.Migrations.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Batches",
                c => new
                    {
                        BatchId = c.Int(nullable: false, identity: true),
                        BatchName = c.String(nullable: false, maxLength: 100),
                        CourseId = c.Int(nullable: false),
                        TrainerId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        TraineeLimit = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.BatchId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Trainers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.TrainerId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseTitle = c.String(nullable: false, maxLength: 50),
                        Overview = c.String(nullable: false, maxLength: 2000),
                        CategoryId = c.Int(nullable: false),
                        CourseHour = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, storeType: "money"),
                        Thumbnail = c.String(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryTitle = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        BatchId = c.Int(nullable: false),
                        TraineeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BatchId, t.TraineeId })
                .ForeignKey("dbo.Batches", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.Trainees", t => t.TraineeId, cascadeDelete: true)
                .Index(t => t.BatchId)
                .Index(t => t.TraineeId);
            
            CreateTable(
                "dbo.Trainees",
                c => new
                    {
                        TraineeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FatherName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Address = c.String(nullable: false, maxLength: 50),
                        NID = c.String(nullable: false, maxLength: 50),
                        BirthDate = c.DateTime(nullable: false, storeType: "date"),
                        PhotoPath = c.String(),
                    })
                .PrimaryKey(t => t.TraineeId);
            
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        TrainerId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FatherName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Address = c.String(nullable: false, maxLength: 50),
                        NID = c.String(nullable: false, maxLength: 50),
                        BirthDate = c.DateTime(nullable: false, storeType: "date"),
                        PhotoPath = c.String(),
                    })
                .PrimaryKey(t => t.TrainerId);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        SkillId = c.Int(nullable: false, identity: true),
                        SkillTitle = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.SkillId);
            
            CreateTable(
                "dbo.TrainerSkills",
                c => new
                    {
                        TrainerId = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TrainerId, t.SkillId })
                .ForeignKey("dbo.Trainers", t => t.TrainerId, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.SkillId, cascadeDelete: true)
                .Index(t => t.TrainerId)
                .Index(t => t.SkillId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Batches", "TrainerId", "dbo.Trainers");
            DropForeignKey("dbo.TrainerSkills", "SkillId", "dbo.Skills");
            DropForeignKey("dbo.TrainerSkills", "TrainerId", "dbo.Trainers");
            DropForeignKey("dbo.Enrollments", "TraineeId", "dbo.Trainees");
            DropForeignKey("dbo.Enrollments", "BatchId", "dbo.Batches");
            DropForeignKey("dbo.Batches", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "CategoryId", "dbo.Categories");
            DropIndex("dbo.TrainerSkills", new[] { "SkillId" });
            DropIndex("dbo.TrainerSkills", new[] { "TrainerId" });
            DropIndex("dbo.Enrollments", new[] { "TraineeId" });
            DropIndex("dbo.Enrollments", new[] { "BatchId" });
            DropIndex("dbo.Courses", new[] { "CategoryId" });
            DropIndex("dbo.Batches", new[] { "TrainerId" });
            DropIndex("dbo.Batches", new[] { "CourseId" });
            DropTable("dbo.TrainerSkills");
            DropTable("dbo.Skills");
            DropTable("dbo.Trainers");
            DropTable("dbo.Trainees");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Categories");
            DropTable("dbo.Courses");
            DropTable("dbo.Batches");
        }
    }
}
