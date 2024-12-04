namespace DevSkill.Inventory.Web.Areas.Admin.Models
{

    public enum ResponseType
    {
        success,
        Danger
    }

    public class ResponseModel
    {
        public string? Message { get; set; }

        public ResponseType Type { get; set;}
    }
}
