using LibraryManagement.Api.DTOs;

namespace LibraryManagement.Api.Services
{
    public interface IMemberService
    {
        List<MemberDto> GetAllMembers();

        MemberDto? GetMemberById(int id);

        MemberDto CreateMember(CreateMemberDto dto);

        MemberDto? UpdateMember(int id, UpdateMemberDto dto);

        bool DeleteMember(int id);
        string? ValidateCreateMember(CreateMemberDto dto);

        string? ValidateUpdateMember(int id, UpdateMemberDto dto);
    }
}