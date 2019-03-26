using WebApp.Model.Models;
using WebApp.Web.Models;
using WebApp.Web.Models.AppUser;

namespace WebApp.Web.Infrastructure.Extensions
{
    public static class EntityExtension
    {
        // --- Identity ---
        public static void MapCreateAppUser(this AppUser user, CreateAppUserVM userVM)
        {
            user.UserName = userVM.UserName;
            user.Email = userVM.Email;
            user.PhoneNumber = userVM.PhoneNumber;

            user.Fullname = userVM.Fullname;
            user.Gender = userVM.Gender;
            user.Address = userVM.Address;

            user.GroupId = userVM.GroupId;
        }

        public static void MapAppUser(this AppUser user, AppUserVM userVM)
        {
            user.Email = userVM.Email;
            user.PhoneNumber = userVM.PhoneNumber;

            user.Fullname = userVM.Fullname;
            user.Gender = userVM.Gender;
            user.Address = userVM.Address;

            user.GroupId = userVM.GroupId;
        }

        public static void MapAppGroup(this AppGroup group, AppGroupVM groupVM)
        {
            group.GroupName = groupVM.GroupName;
            group.Level = groupVM.Level;
            group.SortOrder = groupVM.SortOrder;
        }

        public static void MapAppRole(this AppRole role, AppRoleVM roleVM)
        {
            role.Title = roleVM.Title;
            role.RegionId = roleVM.RegionId;
        }

        public static void MapAppRegion(this AppRegion region, AppRegionVM regionVM)
        {
            region.Title = regionVM.Title;
            region.SortOrder = regionVM.SortOrder;
        }


        public static void MapCreateStudent(this AppUser user, StudentVM userVM)
        {
            user.UserName = userVM.UserName;
            user.Email = userVM.Email;
            user.PhoneNumber = userVM.PhoneNumber;

            user.Fullname = userVM.Fullname;
            user.Gender = userVM.Gender;
            user.Address = userVM.Address;

            user.NativeLand = userVM.NativeLand;
            user.SchoolYear = userVM.SchoolYear;

            user.GroupId = 5;
        }

        public static void MapStudent(this AppUser user, StudentVM userVM)
        {
            user.Email = userVM.Email;
            user.PhoneNumber = userVM.PhoneNumber;

            user.Fullname = userVM.Fullname;
            user.Gender = userVM.Gender;
            user.Address = userVM.Address;

            user.NativeLand = userVM.NativeLand;
            user.SchoolYear = userVM.SchoolYear;
        }

        // --- Content ---

        public static void MapBillElectric(this BillElectric billElectric, BillElectricVM billElectricVM)
        {
            billElectric.OfMonth = billElectricVM.OfMonth;
            billElectric.OfYear = billElectricVM.OfYear;
            billElectric.IndexFirst = billElectricVM.IndexFirst;
            billElectric.IndexLast = billElectricVM.IndexLast;
            billElectric.Amount = billElectricVM.Amount;
            billElectric.IsPaid = billElectricVM.IsPaid;
            billElectric.RoomId = billElectricVM.RoomId;
        }

        public static void MapBillRoom(this BillRoom billRoom, BillRoomVM billRoomVM)
        {
            billRoom.DateFrom = billRoomVM.DateFrom;
            billRoom.DateTo = billRoomVM.DateTo;
            billRoom.Amount = billRoomVM.Amount;
            billRoom.RoomId = billRoomVM.RoomId;
            billRoom.Id = billRoomVM.Id;
        }

        public static void MapBillWater(this BillWater billWater, BillWaterVM billWaterVM)
        {
            billWater.OfMonth = billWaterVM.OfMonth;
            billWater.OfYear = billWaterVM.OfYear;
            billWater.IndexFirst = billWaterVM.IndexFirst;
            billWater.IndexLast = billWaterVM.IndexLast;
            billWater.Amount = billWaterVM.Amount;
            billWater.IsPaid = billWaterVM.IsPaid;
            billWater.RoomId = billWaterVM.RoomId;
        }

        public static void MapDiscipline(this Discipline discipline, DisciplineVM disciplineVM)
        {
            discipline.Reason = disciplineVM.Reason;
            discipline.Description = disciplineVM.Description;
            discipline.AtDate = disciplineVM.AtDate;
            discipline.Id = disciplineVM.Id;
        }

        public static void MapFeedbackAnswer(this FeedbackAnswer feedbackAnswer, FeedbackAnswerVM feedbackAnswerVM)
        {
            feedbackAnswer.Content = feedbackAnswerVM.Content;
        }

        public static void MapFeedback(this Feedback feedback, FeedbackVM feedbackVM)
        {
            feedback.Title = feedbackVM.Title;
            feedback.Content = feedbackVM.Content;
            feedback.IsIncognito = feedbackVM.IsIncognito;
        }

        public static void MapFurniture(this Furniture furniture, FurnitureVM furnitureVM)
        {
            furniture.Reason = furnitureVM.Reason;
            furniture.Description = furnitureVM.Description;
            furniture.RepairDate = furnitureVM.RepairDate;
            furniture.Amount = furnitureVM.Amount;
            furniture.IsPaid = furnitureVM.IsPaid;
            furniture.RoomId = furnitureVM.RoomId;
        }

        public static void MapIndenture(this Indenture indenture, IndentureVM indentureVM)
        {
            indenture.Code = indentureVM.Code;
            indenture.DateFrom = indentureVM.DateFrom;
            indenture.DateTo = indentureVM.DateTo;
            indenture.Id = indentureVM.Id;
            indenture.RoomId = indentureVM.RoomId;
        }

        public static void MapRoomType(this RoomType roomType, RoomTypeVM roomTypeVM)
        {
            roomType.Title = roomTypeVM.Title;
            roomType.Description = roomTypeVM.Description;
            roomType.SortOrder = roomTypeVM.SortOrder;
            roomType.IsActived = roomTypeVM.IsActived;
        }

        public static void MapRoom(this Room room, RoomVM roomVM)
        {
            room.Description = roomVM.Description;
            room.CapacityMax = roomVM.CapacityMax;
            room.SortOrder = roomVM.SortOrder;
            room.IsActived = roomVM.IsActived;
            room.RoomTypeId = roomVM.RoomTypeId;
        }

        public static void MapSwitchRequest(this SwitchRequest switchRequest, SwitchRequestVM switchRequestVM)
        {
            switchRequest.FromRoomId = switchRequestVM.FromRoomId;
            switchRequest.ToRoomId = switchRequestVM.ToRoomId;
            switchRequest.SwitchDate = switchRequestVM.SwitchDate;
            switchRequest.SwitchReason = switchRequestVM.SwitchReason;
        }

    }
}