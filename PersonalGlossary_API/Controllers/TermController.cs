using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.NativeInterop;
using PersonalGlossary_API.Data;
using PersonalGlossary_API.Models;
using PersonalGlossary_API.Models.DTO;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace PersonalGlossary_API.Controllers
{
    [Route("api/term")]
    [ApiController]
    //[Authorize(Roles = "Member, Admin")]
    public class TermController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public TermController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        //[AllowAnonymous]
        //[Authorize(Roles = "Member")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TermDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TermDTO>>>> GetTerms()
        {
            var terms = await _db.Terms.ToListAsync();
            var dtoResponseTerm = _mapper.Map<List<TermDTO>>(terms);
            var response = ApiResponse<IEnumerable<TermDTO>>.Ok(dtoResponseTerm, "Terms retreved successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Member")]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TermDTO>>> GetTermById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Term ID must be greater than 0"));
                }

                var term = await _db.Terms.FirstOrDefaultAsync(u => u.Id == id);

                if (term is null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Term with ID : {id} was not found"));
                }
                return Ok(ApiResponse<object>.Ok(_mapper.Map<TermDTO>(term), "Term retrieved successfully"));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occured while retrieving term with ID {id} : {ex}");
                return StatusCode(500, errorResponse);
            }
            
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TermDTO>>> CreateTerm(TermCreateDTO termDTO)
        {
            try
            {
                if (termDTO is null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Term data is required"));
                }

                var duplicateTerm = await _db.Terms.FirstOrDefaultAsync(u => u.Title.ToLower() ==
                termDTO.Title.ToLower());

                if (duplicateTerm is not null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Term with name {termDTO.Title} already exists"));
                }
                //Term term = new()
                //{
                //    Title = termDTO.Title,
                //    Definition = termDTO.Definition,
                //    CreatedDate = DateTime.Now
                //};
                Term term = _mapper.Map<Term>(termDTO);

                await _db.Terms.AddAsync(term);
                await _db.SaveChangesAsync();

                var response = ApiResponse<TermDTO>.CreatedAt(_mapper.Map<TermDTO>(term), "Term created successfully");
                return CreatedAtAction(nameof(CreateTerm), new { id = term.Id }, response);
            }
            catch (Exception ex)
            {

                var errorResponse = ApiResponse<object>.Error(500, "An error occured while creating a term", ex.Message);
                return StatusCode(500, errorResponse);
            } 
        }

        [HttpPut("{id:int}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<TermDTO>>> UpdateTerm(int id, TermUpdateDTO termUpdateDTO)
        {
            try
            {
                if (termUpdateDTO is null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Term data is required"));
                }
                if (id != termUpdateDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Term ID in URL does not match Term ID in request body"));
                }
                var existingTerm = await _db.Terms.FirstOrDefaultAsync(u => u.Id == id);

                if (existingTerm is null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Term with ID {id} was not found")); 
                }

                var duplicateTerm = await _db.Terms.FirstOrDefaultAsync(u => u.Title.ToLower() == 
                termUpdateDTO.Title.ToLower() && u.Id != id);
                if (duplicateTerm is not null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Term with name {termUpdateDTO.Title} already exists"));
                }

                _mapper.Map(termUpdateDTO, existingTerm);
                existingTerm.UpdatedDate = DateTime.Now;

                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.Ok(_mapper.Map<TermDTO>(termUpdateDTO), "Term updated successfully");
                return Ok(termUpdateDTO);
            }
            catch (Exception ex)
            {

                var errorResponse = ApiResponse<object>.Error(500, "An error occured while updating a term a term", ex.Message);
                return StatusCode(500, errorResponse);
            }
            
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<TermDTO>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTerm(int id)
        {
            try
            {
                var existingTerm = await _db.Terms.FirstOrDefaultAsync(u => u.Id == id);
                if (existingTerm is null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Term with ID {id} was not found"));
                }

                _db.Terms.Remove(existingTerm);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("Term deleted successully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occured while deleting a term", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
