using AutoMapper;
using Model = Pims.Api.Areas.Tools.Models.Import;
using Entity = Pims.Dal.Entities;
using Pims.Api.Profiles.Converters;
using Pims.Api.Areas.Tools.Profiles.Import.Resolvers;

namespace Pims.Api.Areas.Tools.Profiles.Import
{
    public class ParcelProfile : Profile
    {
        #region Constructors
        public ParcelProfile()
        {
            CreateMap<Entity.Parcel, Model.ParcelModel>()
                .ForMember(dest => dest.PID, opt => opt.MapFrom(src => src.ParcelIdentity))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.Classification, opt => opt.MapFrom(src => src.Classification.Name))
                .ForMember(dest => dest.Agency, opt => opt.ConvertUsing(new ParcelAgencyConverter()))
                .ForMember(dest => dest.SubAgency, opt => opt.MapFrom<ParcelSubAgencyResolver>())
                .IncludeBase<Entity.BaseEntity, Pims.Api.Models.BaseModel>();

            CreateMap<Model.ParcelModel, Entity.Parcel>()
                .ForMember(dest => dest.PID, opt => opt.ConvertUsing(new ParcelIdConverter(), src => src.PID))
                .ForMember(dest => dest.ParcelIdentity, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Classification, opt => opt.Ignore())
                .ForMember(dest => dest.Agency, opt => opt.Ignore())
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Address.Id))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.IsSensitive, opt => opt.MapFrom(src => src.IsSensitive))
                .ForMember(dest => dest.Buildings, opt => opt.Ignore())
                .ForMember(dest => dest.Evaluations, opt => opt.Ignore())
                .IncludeBase<Pims.Api.Models.BaseModel, Entity.BaseEntity>();
        }
        #endregion
    }
}
