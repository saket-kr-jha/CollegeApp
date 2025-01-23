namespace DotNetCore_New.Data
{
    public class RolePrivilage
    {
        public int Id { get; set; }
        public string RolePrivilageName { get; set; }
        public string Description { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Role Role { get; set; }
    }
}
