namespace dndsitgen.Serveces.Scenaries
{
    public static class ScenaryFactory
    {
        public static Scenary Create(string name)
        {
            return name switch
            {
                "Boss" => new BossScenary(),
                "Minions" => new MinionsScenary(),
                "Uniform" => new UniformScenary(),
                "Root" => new RootScenary(),
                "Linear" => new LinearScenary(),
                "SuperBoss" => new SuperBossScenary(),
                _ => new MinionsScenary()
            };
        }


    }
}
