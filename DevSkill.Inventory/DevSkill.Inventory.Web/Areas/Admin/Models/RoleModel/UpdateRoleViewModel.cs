namespace DevSkill.Inventory.Web.Areas.Admin.Models.RoleModel
{
    public class UpdateRoleViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}
