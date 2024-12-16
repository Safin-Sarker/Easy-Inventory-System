namespace DevSkill.Inventory.Web.Areas.Admin.Models.User_Models
{
    public class UserProfileModel
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public string Role { get; set; }
        public string? Gender { get; set; }
        public string? BirthDate { get; set; }
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? TimeZone { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
