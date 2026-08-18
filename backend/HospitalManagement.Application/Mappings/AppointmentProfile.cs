using AutoMapper;
using HospitalManagement.Application.DTO;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Mappings
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            // Create DTO -> Entity
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(
                    dest => dest.AppointmentId,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => AppointmentStatus.Scheduled))

                .ForMember(
                    dest => dest.CreatedAt,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Patient,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Doctor,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Prescription,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Bill,
                    opt => opt.Ignore());


            // Entity -> Response DTO
            CreateMap<Appointment, AppointmentResponseDto>()
                .ForMember(
                    dest => dest.PatientName,
                    opt => opt.MapFrom(src =>
                        src.Patient != null
                            ? src.Patient.FullName
                            : string.Empty))

                .ForMember(
                    dest => dest.DoctorName,
                    opt => opt.MapFrom(src =>
                        src.Doctor != null
                            ? src.Doctor.FullName
                            : string.Empty));


            // Update DTO -> Entity
            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(
                    dest => dest.AppointmentId,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.PatientId,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.DoctorId,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Status,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.CreatedAt,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Patient,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Doctor,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Prescription,
                    opt => opt.Ignore())

                .ForMember(
                    dest => dest.Bill,
                    opt => opt.Ignore());
        }
    }
}