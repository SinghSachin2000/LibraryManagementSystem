using LibraryManagement.Api.Data;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using System.Net.Mail;

namespace LibraryManagement.Api.Services
{
    public class MemberService : IMemberService
    {
        private readonly LibraryDbContext _context;

        public MemberService(LibraryDbContext context)
        {
            _context = context;
        }

        public List<MemberDto> GetAllMembers()
        {
            return _context.Members
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Email = m.Email,
                    JoinedOn = m.JoinedOn,
                    IsActive = m.IsActive
                })
                .ToList();
        }

        public MemberDto? GetMemberById(int id)
        {
            return _context.Members
                .Where(m => m.Id == id)
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Email = m.Email,
                    JoinedOn = m.JoinedOn,
                    IsActive = m.IsActive
                })
                .FirstOrDefault();
        }
        public string? ValidateCreateMember(CreateMemberDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                return "Full name is required";

            if (string.IsNullOrWhiteSpace(dto.Email))
                return "Email is required";

            //MailAddress.TryCreate() email ka basic format verify karta hai.
            if (!System.Net.Mail.MailAddress.TryCreate(dto.Email, out _))
                return "Please enter a valid email address";

            bool emailExists = _context.Members.Any(m =>
                m.Email.ToLower() == dto.Email.ToLower());

            if (emailExists)
                return "A member with this email already exists";

            return null;
        }

        public MemberDto CreateMember(CreateMemberDto dto)
        {
            var member = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email,
                JoinedOn = DateTime.Now,
                IsActive = true
            };

            _context.Members.Add(member);
            _context.SaveChanges();

            return GetMemberById(member.Id)!;
        }
        public string? ValidateUpdateMember(int id, UpdateMemberDto dto)
        {
            var member = _context.Members.FirstOrDefault(m => m.Id == id);

            if (member == null)
                return "Member not found";

            if (string.IsNullOrWhiteSpace(dto.FullName))
                return "Full name is required";

            if (string.IsNullOrWhiteSpace(dto.Email))
                return "Email is required";

            if (!System.Net.Mail.MailAddress.TryCreate(dto.Email, out _))
                return "Please enter a valid email address";

            bool emailExists = _context.Members.Any(m =>
                m.Id != id &&
                m.Email.ToLower() == dto.Email.ToLower());

            if (emailExists)
                return "A member with this email already exists";

            return null;
        }
        public MemberDto? UpdateMember(int id, UpdateMemberDto dto)
        {
            var member = _context.Members.FirstOrDefault(m => m.Id == id);

            if (member == null)
                return null;

            member.FullName = dto.FullName;
            member.Email = dto.Email;
            member.IsActive = dto.IsActive;

            _context.SaveChanges();

            return GetMemberById(id);
        }

        public bool DeleteMember(int id)
        {
            var member = _context.Members.FirstOrDefault(m => m.Id == id);

            if (member == null)
                return false;

            _context.Members.Remove(member);
            _context.SaveChanges();

            return true;
        }
    }
}