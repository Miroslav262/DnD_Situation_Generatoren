namespace dndsitgen.Models
{
    public class CreatureCr
    {
        public int Id { get; set; }
        public string Cr { get; set; } = "";
        public string Exp { get; set; } = "";
        public int ProficiencyBonus { get; set; }
    }

    public class CreatureAction
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Text { get; set; } = "";
    }

    public class CreatureTrait
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Text { get; set; } = "";
    }

    public class CreatureLegendary
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Text { get; set; } = "";
    }

    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string PassHash { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.User;
    }

    public class BattleScene
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreatureSize
    {
        public int Id { get; set; }
        public string Letter { get; set; } = "";
        public string Name { get; set; } = "";

    }


}


