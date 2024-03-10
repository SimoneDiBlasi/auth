using AutoMapper;

namespace auth.Handlers.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Models.Signup.Address, Model.Address>();
        }
    }
}
