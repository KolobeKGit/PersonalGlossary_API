using PersonalGlossary_API.Models.DTO;

namespace PersonalGlossaryWeb.Services.IServices
{
    public interface ITermService
    {
        Task<T?> GetAllAsync<T>(string token);
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateAsync<T>(TermCreateDTO dto);
        Task<T?> UpdateAsync<T>(TermUpdateDTO dto);
        Task<T?> DeleteAsync<T>(int id);
    }
}
