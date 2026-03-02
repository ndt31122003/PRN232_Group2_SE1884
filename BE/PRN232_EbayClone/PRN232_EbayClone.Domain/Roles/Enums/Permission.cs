using System.ComponentModel;

namespace PRN232_EbayClone.Domain.Roles.Enums;

public enum Permission
{
    [Description("Xem danh sách người dùng")]
    ViewUser,

    [Description("Chỉnh sửa thông tin người dùng")]
    EditUser,

    [Description("Xoá người dùng")]
    DeleteUser,

    [Description("Gán quyền cho Role")]
    AssignRole,

    CreateRole
}
