using System.ComponentModel.DataAnnotations;

namespace DotNetCore_New.Model
{
    public class RolePrivilageDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        [Required]
        public string RolePrivilageName { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
