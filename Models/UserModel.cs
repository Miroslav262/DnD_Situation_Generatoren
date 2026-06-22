namespace dndsitgen.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public string pass_hash { get; set; } = "";
    }
}
