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
            Delete.Table("outgoings");
            Delete.Table("income");
            Delete.Table("products");
            Delete.Table("companies");
            Delete.Table("users");

        }

        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("userName").AsString(50)
                .WithColumn("eMail").AsString(50)
                .WithColumn("password_hash").AsString(25);

            Create.Table("companies")
                .WithColumn("userID").AsInt32().ForeignKey("users", "ID")
                .WithColumn("companyID").AsInt32().Identity().PrimaryKey()
                .WithColumn("companyName").AsString(128)
                .WithColumn("address").AsString(256)
                .WithColumn("phoneNumber").AsString(15) 
                .WithColumn("faxNumber").AsString(15)
                .WithColumn("TaxAdministration").AsString(15)
                .WithColumn("IBAN").AsString(32); // longest IBAN belongs to Saint Lucia

            Create.Table("products")
                .WithColumn("userID").AsInt32().ForeignKey("users", "ID")
                .WithColumn("productID").AsInt32().Identity().PrimaryKey()
                .WithColumn("productName").AsString(256)
                .WithColumn("inStock").AsInt32()
                .WithColumn("priceBeforeTax").AsInt32()
                .WithColumn("taxRate").AsInt32()
                .WithColumn("priceAfterTax").AsInt32();

            Create.Table("income")
                .WithColumn("userID").AsInt32().ForeignKey("users", "ID")
                .WithColumn("incomeID").AsInt32().Identity().PrimaryKey()
                .WithColumn("date").AsDate()
                .WithColumn("companyID").AsInt32().ForeignKey("companies", "companyID")
                .WithColumn("explanation").AsString(512)
                .WithColumn("productID").AsInt32().ForeignKey("products", "productID")
                .WithColumn("quantity").AsInt32()
                .WithColumn("total").AsInt32();

            Create.Table("outgoings")
                .WithColumn("userID").AsInt32().ForeignKey("users","ID")
                .WithColumn("outgoingID").AsInt32().Identity().PrimaryKey()
                .WithColumn("date").AsDate()
                .WithColumn("companyID").AsInt32().ForeignKey("companies", "companyID")
                .WithColumn("explanation").AsString(512)
                .WithColumn("productID").AsInt32().ForeignKey("products", "productID")
                .WithColumn("quantity").AsInt32()
                .WithColumn("total").AsInt32();


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