namespace GameStore.Mapping;

/// <summary>
/// Profilo AutoMapper per le mappature tra entità e DTO
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mappature per Utente
        CreateMap<Utente, UtenteDto>();
        CreateMap<CreaUtenteDto, Utente>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.Acquisti, opt => opt.Ignore())
            .ForMember(dest => dest.Recensioni, opt => opt.Ignore());

        CreateMap<AggiornaUtenteDto, Utente>()
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataRegistrazione, opt => opt.Ignore())
            .ForMember(dest => dest.Acquisti, opt => opt.Ignore())
            .ForMember(dest => dest.Recensioni, opt => opt.Ignore());

        // Mappature per Gioco
        CreateMap<Gioco, GiocoDto>();
        CreateMap<CreaGiocoDto, Gioco>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.Acquisti, opt => opt.Ignore())
            .ForMember(dest => dest.Recensioni, opt => opt.Ignore());

        CreateMap<AggiornaGiocoDto, Gioco>()
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.Acquisti, opt => opt.Ignore())
            .ForMember(dest => dest.Recensioni, opt => opt.Ignore());

        // Mappature per Acquisto
        CreateMap<Acquisto, AcquistoDto>()
            .ForMember(dest => dest.UtenteUsername, opt => opt.MapFrom(src => src.Utente.Username))
            .ForMember(dest => dest.GiocoTitolo, opt => opt.MapFrom(src => src.Gioco.Titolo));

        CreateMap<CreaAcquistoDto, Acquisto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.Utente, opt => opt.Ignore())
            .ForMember(dest => dest.Gioco, opt => opt.Ignore())
            .ForMember(dest => dest.Recensioni, opt => opt.Ignore());

        CreateMap<AggiornaAcquistoDto, Acquisto>()
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.Utente, opt => opt.Ignore())
            .ForMember(dest => dest.Gioco, opt => opt.Ignore())
            .ForMember(dest => dest.Recensioni, opt => opt.Ignore());

        // Mappature per Recensione
        CreateMap<Recensione, RecensioneDto>()
            .ForMember(dest => dest.UtenteUsername, opt => opt.MapFrom(src => src.Utente.Username))
            .ForMember(dest => dest.GiocoTitolo, opt => opt.MapFrom(src => src.Gioco.Titolo));

        CreateMap<CreaRecensioneDto, Recensione>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.Utente, opt => opt.Ignore())
            .ForMember(dest => dest.Gioco, opt => opt.Ignore())
            .ForMember(dest => dest.Acquisto, opt => opt.Ignore());

        CreateMap<AggiornaRecensioneDto, Recensione>()
            .ForMember(dest => dest.DataCreazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataUltimaModifica, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancellato, opt => opt.Ignore())
            .ForMember(dest => dest.DataCancellazione, opt => opt.Ignore())
            .ForMember(dest => dest.DataRecensione, opt => opt.Ignore())
            .ForMember(dest => dest.Utente, opt => opt.Ignore())
            .ForMember(dest => dest.Gioco, opt => opt.Ignore())
            .ForMember(dest => dest.Acquisto, opt => opt.Ignore());
    }
}
