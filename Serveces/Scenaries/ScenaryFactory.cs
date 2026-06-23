namespace dndsitgen.Serveces.Scenaries
{
    public static class ScenaryFactory
    {
        public static Scenary Create(ScenaryEnum scenary)
        {
            return scenary switch
            {
                ScenaryEnum.Boss => new BossScenary(),
                ScenaryEnum.Minions => new MinionsScenary(),
                ScenaryEnum.Uniform => new UniformScenary(),
                ScenaryEnum.Root => new RootScenary(),
                ScenaryEnum.Linear => new LinearScenary(),
                ScenaryEnum.SuperBoss => new SuperBossScenary(),
                _ => new MinionsScenary()
            };
        }


    }
}
