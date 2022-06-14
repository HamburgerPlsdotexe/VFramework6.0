using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using Constructs;

namespace VFBlazor6._0.TerraformLogic
{
    internal class Databases : TerraformStack
    {
        internal readonly PostgresqlDatabase? _postgresqlDatabase;

        internal Databases(Construct scope, string id, Namegenerator ng, ResourceGroups rg, string database="postgres") : base(scope, id)
        {
            switch (database)
            {
                case "postgres":
                    PostgresqlDatabase postgresqlDatabase = new(this, "azurerm_postgresql_Database", new PostgresqlDatabaseConfig
                    {
                        //Name = 
                    });
                    break;
                
                default:
                    break;
            }
        }
    }
}
