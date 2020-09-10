namespace DAL
{
    public class RegionRepository : Repository<Models.Region>, IRegionRepository
    {
        public RegionRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
        }
    }
}
