using AutoMapper;
using WebApp.Model.Models;
using WebApp.Web.Models;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Infrastructure.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(config =>
            {
                // --- Identity ---
                config.CreateMap<AppUser, AppUserVM>();
                config.CreateMap<AppGroup, AppGroupVM>();
                config.CreateMap<AppRole, AppRoleVM>();
                config.CreateMap<AppRegion, AppRegionVM>();
                config.CreateMap<AppUser, StudentVM>();

                // --- Content ---
                config.CreateMap<RoomType, RoomTypeVM>();
                config.CreateMap<Room, RoomVM>();
                config.CreateMap<Indenture, IndentureVM>();
                config.CreateMap<BillRoom, BillRoomVM>();
                config.CreateMap<BillElectric, BillElectricVM>();
                config.CreateMap<BillWater, BillWaterVM>();
                config.CreateMap<Furniture, FurnitureVM>();
                config.CreateMap<Discipline, DisciplineVM>();
                config.CreateMap<Feedback, FeedbackVM>();
                config.CreateMap<FeedbackAnswer, FeedbackAnswerVM>();
                config.CreateMap<Message, MessageVM>();
                config.CreateMap<SwitchRequest, SwitchRequestVM>();
            });

        }
    }
}