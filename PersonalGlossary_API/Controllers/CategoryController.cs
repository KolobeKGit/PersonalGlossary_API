using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalGlossary_API.Data;
using PersonalGlossary_API.Models;
using PersonalGlossary_API.Models.DTO;

namespace PersonalGlossary_API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CategoryController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDTO>>>> GetAllCategories()
        {
            var categories = await _db.Categories.ToListAsync();
            var dtoResponseCategories = _mapper.Map<List<CategoryDTO>>(categories);
            var response = ApiResponse<IEnumerable<CategoryDTO>>.Ok(dtoResponseCategories, "Categories retrived successfully");
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                if (categoryCreateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Category data is required"));
                }

                var termExists = await _db.Terms.FirstOrDefaultAsync(u => u.Id == categoryCreateDTO.TermId);

                if (termExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Term with ID:{categoryCreateDTO.TermId} does not exist"));
                }

                Category category = _mapper.Map<Category>(categoryCreateDTO);
                category.CreatedDate = DateTime.Now;
                await _db.Categories.AddAsync(category);
                await _db.SaveChangesAsync();

                var response = ApiResponse<CategoryDTO>.CreatedAt(_mapper.Map<CategoryDTO>(category), "Category created successfully");
                return CreatedAtAction(nameof(CreateCategory), new { id = category.Id }, response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occured while creating a category", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryCreateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> GetCategoryById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Category ID must be greater than 0"));
                }

                var category = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);
                if (category == null)
                {
                    return NotFound(ApiResponse<CategoryDTO>.NotFound($"Category with ID:{id} was not found"));
                }
                return Ok(ApiResponse<CategoryDTO>.Ok(_mapper.Map<CategoryDTO>(category), "Categoty retrieved successfully"));
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An Error while retrieving data with ID{id}:", ex.Message);
                return StatusCode(500, errorResponse);
            }
           
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> UpdateCategory(int id , CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                if (categoryUpdateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Category ID in URL does not match category ID in request body"));
                }
                if (id != categoryUpdateDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Category ID in URL does not match categoty ID in request body"));
                }

                //Also check ID of the foreign key
                var termExists = await _db.Terms.FirstOrDefaultAsync(u => u.Id == categoryUpdateDTO.TermId);
                if (termExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Term with ID{categoryUpdateDTO.TermId} does not exist"));
                }

                var existingCategory = await _db.Categories.FirstOrDefaultAsync((u => u.Id == id));
                if (existingCategory == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Category with ID {id} was not found"));
                }

                _mapper.Map(categoryUpdateDTO, existingCategory);
                existingCategory.UpdatedDate = DateTime.Now;
                await _db.SaveChangesAsync();

                var response = ApiResponse<CategoryDTO>.Ok(_mapper.Map<CategoryDTO>(existingCategory), "Category updated successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<CategoryDTO>.Error(500, "An error occured while updating a category", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCategory(int id)
        {
            try
            {
                var exisitingCategory = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);

                if (exisitingCategory == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Category with ID {id} was not found"));
                }
                _db.Categories.Remove(exisitingCategory);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("Category deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occured while deleting a category");
                return StatusCode(500, errorResponse);
            }
            
        }
    }
}
