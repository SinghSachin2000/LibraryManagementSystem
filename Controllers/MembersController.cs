using LibraryManagement.Api.Data;
using LibraryManagement.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Api.DTOs;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        public MembersController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllMembers()
        {
            //var members = _context.Members.ToList();
            var members = _context.Members.Select(m=>new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JoinedOn = m.JoinedOn,
                IsActive = m.IsActive
            }).ToList();
            return Ok(members);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetMemberById(int id)
        {
            var member = _context.Members.Where(m=>m.Id==id).Select(m=>new MemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                JoinedOn = m.JoinedOn,
                IsActive = m.IsActive
            }).FirstOrDefault();
            if (member == null)
            {
                return NotFound(new { message = "member is not found" });
            }
            return Ok(member);
        }

        [HttpPost]
        public IActionResult AddMember([FromBody] CreateMemberDto member)
        {
            if (string.IsNullOrWhiteSpace(member.FullName))
            {
                return BadRequest(new { message = "fullname is required" });
            }
            if (string.IsNullOrWhiteSpace(member.Email))
            {
                return BadRequest(new { message = "email is required" });
            }
            var isEmailExists = _context.Members.Any(m => m.Email.ToLower() == member.Email.ToLower());
            if (isEmailExists)
            {
                return BadRequest(new { message = "A member with this email is already exists" });
            }
            //member.Id = 0;
            //member.JoinedOn = DateTime.Now;
            //member.IsActive = true;

            var newMember =new Member
            {
                FullName = member.FullName,
                Email = member.Email,
                JoinedOn = DateTime.Now,
                IsActive = true
            };

            _context.Members.Add(newMember);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMemberById),
                new { id = newMember.Id },
                new MemberDto
                {
                    Id = newMember.Id,
                    FullName = newMember.FullName,
                    Email = newMember.Email,
                    JoinedOn = newMember.JoinedOn,
                    IsActive = newMember.IsActive
                });

        }

        [HttpPut("{id:int}")]
        public IActionResult updateMember(int id, [FromBody] UpdateMemberDto member)
        {
            var existingMember = _context.Members.FirstOrDefault(m => m.Id == id);
            if (existingMember==null)
            {
                return NotFound(new
                {
                    message = "this member is not found"
                });
            }
            if (string.IsNullOrWhiteSpace(member.FullName))
                return BadRequest(new { message = "FullName is required." });

            if (string.IsNullOrWhiteSpace(member.Email))
                return BadRequest(new { message = "Email is required." });

            bool emailExists = _context.Members.Any(m =>
                m.Id != id &&
                m.Email.ToLower() == member.Email.ToLower());

            if (emailExists)
                return BadRequest(new { message = "Email already exists." });

            existingMember.FullName = member.FullName;
            existingMember.Email = member.Email;
            existingMember.IsActive = member.IsActive;

            _context.SaveChanges();

            return Ok(new MemberDto
            {
                Id = existingMember.Id,
                FullName = existingMember.FullName,
                Email = existingMember.Email,
                JoinedOn = existingMember.JoinedOn,
                IsActive = existingMember.IsActive
            });
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteMember(int id)
        {
            var member = _context.Members.FirstOrDefault(m => m.Id == id);

            if (member == null)
            {
                return NotFound(new
                {
                    message = $"Member with ID {id} was not found."
                });
            }

            _context.Members.Remove(member);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
