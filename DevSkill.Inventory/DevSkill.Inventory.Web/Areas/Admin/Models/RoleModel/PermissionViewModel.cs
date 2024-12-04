namespace DevSkill.Inventory.Web.Areas.Admin.Models.RoleModel
{
    public class PermissionViewModel
    {
        public string Name { get; set; }         // Internal name of the permission (used as value in the checkbox)
        public string DisplayName { get; set; }
        public string Category { get; set; } // User-friendly name displayed next to the checkbox
        public bool IsSelected { get; set; }
    }
}
