using PersonalGlossary_API.Models.DTO;
using PersonalGlossaryWeb.Models;

namespace PersonalGlossaryWeb.Services.IServices
{
    public interface IBaseService
    {
        ApiResponse<object> ResponseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
