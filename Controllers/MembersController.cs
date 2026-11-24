using LibraryManagement.Api.Data;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        //private readonly LibraryDbContext _context;
        //public MembersController(LibraryDbContext context)
        //{
        //    _context = context;
        //}
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        [HttpGet]
        public IActionResult GetAllMembers()
        {
            //var members = _context.Members.ToList();
            var members = _memberService.GetAllMembers();
            return Ok(members);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetMemberById(int id)
        {
          var member = _memberService.GetMemberById(id);

            if (member == null)
            {
                return NotFound(new { message = "Member not found" });
            }

            return Ok(member);
        }

        [HttpPost]
        public IActionResult AddMember([FromBody] CreateMemberDto member)
        {
            //if (string.IsNullOrWhiteSpace(member.FullName))
            //{
            //    return BadRequest(new { message = "fullname is required" });
            //}
            //if (string.IsNullOrWhiteSpace(member.Email))
            //{
            //    return BadRequest(new { message = "email is required" });
            //}
            //var isEmailExists = _context.Members.Any(m => m.Email.ToLower() == member.Email.ToLower());
            //if (isEmailExists)
            //{
            //    return BadRequest(new { message = "A member with this email is already exists" });
            //}
            //member.Id = 0;
            //member.JoinedOn = DateTime.Now;
            //member.IsActive = true;

            //var newMember =new Member
            //{
            //    FullName = member.FullName,
            //    Email = member.Email,
            //    JoinedOn = DateTime.Now,
            //    IsActive = true
            //};

            //_context.Members.Add(newMember);
            //_context.SaveChanges();

            //return CreatedAtAction(nameof(GetMemberById),
            //    new { id = newMember.Id },
            //    new MemberDto
            //    {
            //        Id = newMember.Id,
            //        FullName = newMember.FullName,
            //        Email = newMember.Email,
            //        JoinedOn = newMember.JoinedOn,
            //        IsActive = newMember.IsActive
            //    });

            var error = _memberService.ValidateCreateMember(member);

            if (error != null)
            {
                return BadRequest(new { message = error });
            }

            var newmMember = _memberService.CreateMember(member);

            return CreatedAtAction(
                nameof(GetMemberById),
                new { id = newmMember.Id },
                newmMember
            );

        }

        [HttpPut("{id:int}")]
        public IActionResult updateMember(int id, [FromBody] UpdateMemberDto member)
        {
            //var existingMember = _context.Members.FirstOrDefault(m => m.Id == id);
            //if (existingMember==null)
            //{
            //    return NotFound(new
            //    {
            //        message = "this member is not found"
            //    });
            //}
            //if (string.IsNullOrWhiteSpace(member.FullName))
            //    return BadRequest(new { message = "FullName is required." });

            //if (string.IsNullOrWhiteSpace(member.Email))
            //    return BadRequest(new { message = "Email is required." });

            //bool emailExists = _context.Members.Any(m =>
            //    m.Id != id &&
            //    m.Email.ToLower() == member.Email.ToLower());

            //if (emailExists)
            //    return BadRequest(new { message = "Email already exists." });

            //existingMember.FullName = member.FullName;
            //existingMember.Email = member.Email;
            //existingMember.IsActive = member.IsActive;

            //_context.SaveChanges();

            //return Ok(new MemberDto
            //{
            //    Id = existingMember.Id,
            //    FullName = existingMember.FullName,
            //    Email = existingMember.Email,
            //    JoinedOn = existingMember.JoinedOn,
            //    IsActive = existingMember.IsActive
            //});
            var error = _memberService.ValidateUpdateMember(id, member);

            if (error != null)
            {
                if (error == "Member not found")
                {
                    return NotFound(new { message = error });
                }

                return BadRequest(new { message = error });
            }

            var updatedMember = _memberService.UpdateMember(id, member);

            return Ok(updatedMember);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteMember(int id)
        {
            //var member = _context.Members.FirstOrDefault(m => m.Id == id);

            //if (member == null)
            //{
            //    return NotFound(new
            //    {
            //        message = $"Member with ID {id} was not found."
            //    });
            //}

            //_context.Members.Remove(member);
            //_context.SaveChanges();
            bool isDeleted = _memberService.DeleteMember(id);

            if (!isDeleted)
                return NotFound(new { message = "Member not found" });


            return NoContent();
        }
    }
}
