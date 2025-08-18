using AutoMapper;
using BookingMaster.Dtos.Accommodation;
using BookingMaster.Dtos.AccommodationProposal;
using BookingMaster.Dtos.Bookings;
using BookingMaster.Dtos.Feedbacks;
using BookingMaster.Models;

namespace BookingMaster.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Accommodation
            CreateMap<Accommodation, AccommodationResponse>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType));
            CreateMap<Accommodation, AccommodationSearchResponse>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType));

            CreateMap<PropertyType, PropertyTypeAccommodationResponse>();

            // AccommodationProposal
            CreateMap<AccommodationProposal, AccommodationProposalResponse>();
            CreateMap<AccommodationProposal, AccommodationProposalOwnerResponse>();

            // Booking
            CreateMap<Booking, BookingCustomerResponse>()
            .ForMember(dest => dest.Accommodation, opt => opt.MapFrom(src => src.AccommodationProposal.Accommodation))
            .ForMember(dest => dest.AccommodationProposal, opt => opt.MapFrom(src => src.AccommodationProposal))
            .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.Feedback))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.NormalizedName));

            CreateMap<Feedback, FeedbackBookingResponse>();
            CreateMap<Accommodation, AccommodationBookingResponse>();
            CreateMap<AccommodationProposal, AccommodationProposalBookingResponse>();

            CreateMap<Booking, BookingOwnerResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.NormalizedName))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.Feedback))
            .ForMember(dest => dest.Accommodation, opt => opt.MapFrom(src => src.AccommodationProposal.Accommodation))
            .ForMember(dest => dest.AccommodationProposal, opt => opt.MapFrom(src => src.AccommodationProposal));

            CreateMap<User, CustomerResponse>();

            // Feedback
            CreateMap<Feedback, FeedbackResponse>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Booking.Customer))
            .ForMember(dest => dest.AccommodationProposal, opt => opt.MapFrom(src => src.Booking.AccommodationProposal));

            CreateMap<User, CustomerFeedbackResponse>();
            CreateMap<AccommodationProposal, AccommodationProposalFeedbackResponse>();
        }
    }
}
