using FluentMigrator;
using FluentMigrator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KalamazoDefteri.Migrations
{
    [Migration(1)]
    public class _001_Users_and_Roles : Migration
    {
        public override void Down()
        {
            Delete.Table("role_users");
            Delete.Table("roles");
            Delete.Table("outgoings");
            Delete.Table("income");
            Delete.Table("companies");
            Delete.Table("users");

        }

        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("userName").AsString(50).NotNullable()
                .WithColumn("eMail").AsString(50).NotNullable()
                .WithColumn("balance").AsInt32().NotNullable()
                .WithColumn("adSoyad").AsString(128).NotNullable()
                .WithColumn("addressMah").AsString(40).NotNullable()
                .WithColumn("addressCadSk").AsString(128).NotNullable()
                .WithColumn("addressil").AsString(128).NotNullable()
                .WithColumn("addressilce").AsString(128).NotNullable()
                .WithColumn("password_hash").AsString(256).NotNullable();

            Create.Table("companies")
                .WithColumn("userID").AsInt32().ForeignKey("users", "Id").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("companyID").AsInt32().Identity().PrimaryKey()
                .WithColumn("companyName").AsString(128).NotNullable()
                .WithColumn("address").AsString(256).NotNullable()
                .WithColumn("phoneNumber").AsString(20).NotNullable()
                .WithColumn("faxNumber").AsString(20).NotNullable()
                .WithColumn("TaxAdministration").AsString(15).NotNullable()
                .WithColumn("IBAN").AsString(32).NotNullable()
                .WithColumn("inBalance").AsInt32().NotNullable()
                .WithColumn("outBalance").AsInt32().NotNullable();

            Create.Table("income")
                .WithColumn("userID").AsInt32().ForeignKey("users", "ID").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("incomeID").AsInt32().Identity().PrimaryKey()
                .WithColumn("date").AsDate().NotNullable()
                .WithColumn("companyID").AsInt32().ForeignKey("companies", "companyID").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("explanation").AsString(512).NotNullable()
                .WithColumn("payment").AsInt32().NotNullable();

            Create.Table("outgoings")
                .WithColumn("userID").AsInt32().ForeignKey("users", "ID").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("outgoingID").AsInt32().Identity().PrimaryKey()
                .WithColumn("date").AsDate().NotNullable()
                .WithColumn("companyID").AsInt32().ForeignKey("companies", "companyID").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("explanation").AsString(512).NotNullable()
                .WithColumn("payment").AsInt32().NotNullable();

            Create.Table("roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128);

            Create.Table("role_users")
                .WithColumn("userid").AsInt32().ForeignKey("users", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("roleid").AsInt32().ForeignKey("roles", "id").OnDelete(System.Data.Rule.Cascade);

            /* CMD line;
             migrate -a C:\Users\Atabay\source\repos\KalamazoDefteri\KalamazoDefteri\bin\KalamazoDefteri.dll -db MySql -conn "Data Source=127.0.0.1;Database=kalamazodefteri;uid=root;pwd=root;"
            *//* CMD line;
             migrate -a C:\Users\Atabay\source\repos\KalamazoDefteri\KalamazoDefteri\bin\KalamazoDefteri.dll -db MySql -conn "Data Source=127.0.0.1;Database=kalamazodefterinew;uid=root;pwd=root;"
            */
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override void GetDownExpressions(IMigrationContext context)
        {
            base.GetDownExpressions(context);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void GetUpExpressions(IMigrationContext context)
        {
            base.GetUpExpressions(context);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}