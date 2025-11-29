namespace Authentication.RefreshToken.Domain.Constants
{
    public static class Permissions
    {
        // Acceso
        public const string UsersView = "users.view";
        public const string UsersManage = "users.manage"; // Incluye Create, Edit, Delete para simplificar la Beta
        public const string RolesView = "roles.view";
        public const string RolesManage = "roles.manage";

        // Catalogo
        public const string ProductsView = "products.view";
        public const string ProductsCreate = "products.create";
        public const string ProductsEdit = "products.edit";
        public const string ProductsDelete = "products.delete";

        // Ventas
        public const string OrdersView = "orders.view";
        public const string OrdersManage = "orders.manage"; // Cambiar estados

        // Analitica
        public const string DashboardView = "dashboard.view";
        public const string ReportsExport = "reports.export"; // Cubre Excel y PDF
    }
}
