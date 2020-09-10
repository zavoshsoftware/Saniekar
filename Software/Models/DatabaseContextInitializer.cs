using System;

namespace Models
{
    internal static class DatabaseContextInitializer
    {
        static DatabaseContextInitializer()
        {

        }

        internal static void Seed(DatabaseContext databaseContext)
        {
             InitialRoles(databaseContext);
           // InitialProductType(databaseContext);
        }

        #region Role
        public static void InitialRoles(DatabaseContext databaseContext)
        {
            InsertRole("f1dcedb2-a865-4c73-bc51-1afd28118d39", "SuperAdministrator", "راهبر ویژه", databaseContext);
            InsertRole("f53d469b-4172-42a9-8355-20032367c627", "Administrator", "راهبر", databaseContext);
            InsertRole("b999eb27-7330-4062-b81f-62b3d1935885", "Branch", "شعب", databaseContext);
        }

        public static void InsertRole(string roleId, string roleName, string roleTitle, DatabaseContext databaseContext)
        {
            Guid id = new Guid(roleId);
            Role role = new Role();
            role.Id = id;
            role.Title = roleTitle;
            role.Name = roleName;
            role.CreationDate = DateTime.Now;
            role.IsActive = true;
            role.IsDeleted = false;

            databaseContext.Roles.Add(role);
            databaseContext.SaveChanges();
        }
        #endregion

      


    }
}
