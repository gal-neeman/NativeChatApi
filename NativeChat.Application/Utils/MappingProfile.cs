using AutoMapper;

namespace NativeChat;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterDto>();
        CreateMap<RegisterDto, User>();
    }
}
