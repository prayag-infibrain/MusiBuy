using Microsoft.AspNetCore.Mvc;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.ResponseModels;

namespace MusiBuy.API.Controllers
{
    [Route("api/List")]
    [ApiController]
    public class ListController : ControllerBase
    {
        #region Member Declaration
        private readonly IDropdown _dropdown;
        private readonly IContentManagement _content;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authenticate"></param>
        public ListController(IDropdown dorpdown, IContentManagement content)
        {
            _dropdown = dorpdown;
            _content = content;
        }
        #endregion

        #region Get Event Type List
        [HttpGet("GetEventTypeList")]
        [EndpointSummary("GetEventTypeList")]
        [EndpointDescription("Get Event type List")]
        [EndpointName("GetEventTypeList")]
        public ActionResult<ApiResponse> GetEventTypeList()
        {
            var result = _dropdown.GetEventTypeList();
            if (result != null)
                return Ok(new ApiResponse(true, "Event Type List", result.Select(r => new {
                    r.Id,
                    r.EnumValue,
                    r.Description,
                    r.IsActive
                })
                    ));

            return Ok(new ApiResponse(false, "Error While Get Event Type List", null));
        }
        #endregion

        #region Get Enum List By Type DropDown List
        [HttpPost("GetEnumByType")]
        [EndpointSummary("GetEnumByType")]
        [EndpointDescription("Get Enum Value For Get Dropdown ")]
        [EndpointName("GetEnumByType")]
        public ActionResult<ApiResponse> GetEnumByType(CommonModel model)
        {
            var result = _dropdown.GetEnumListByType(model.Id);
            if (result != null)
                return Ok(new ApiResponse(true, "Enum List", result.Select(r => new {
                    r.Id,
                    r.EnumValue,
                    r.Description,
                    r.IsActive
                })
                    ));

            return Ok(new ApiResponse(false, "Error While Get List", null));
        }
        #endregion

        #region Get Country DropDown List
        [HttpGet("GetCountry")]
        [EndpointSummary("GetCountry")]
        [EndpointDescription("Get Country List")]
        [EndpointName("GetCountry")]
        public ActionResult<ApiResponse> GetCountry()
        {
            var result = _dropdown.GetCountry();
            if (result != null)
            {
                var finalResult = result.Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Code,
                    r.IsActive,
                    FlagUrl = r.CountryShortCode != null ?  $"https://flagcdn.com/w40/{r.CountryShortCode.ToLower()}.png" : null
                });

                return Ok(new ApiResponse(true, "Country List", finalResult));
            }

            return Ok(new ApiResponse(false, "Error While Get Country List", null));
        }
        #endregion

        #region Get Genre DropDown List
        [HttpGet("GetGenre")]
        [EndpointSummary("GetGenre")]
        [EndpointDescription("Get Genre List")]
        [EndpointName("GetGenre")]
        public ActionResult<ApiResponse> GetGenre()
        {
            var result = _dropdown.GetGenre();
            if (result != null)
                return Ok(new ApiResponse(true, "Genre List", result.Select(r => new {
                    r.Id,
                    r.GenreName,
                    r.Description,
                    r.CountryId,
                    r.CountryName,
                    r.IsActive
                })
                    ));

            return Ok(new ApiResponse(false, "Error While Get Genre List", null));
        }
        #endregion

        #region Get Role DropDown List
        [HttpGet("GetRoleList")]
        [EndpointSummary("GetRoleList")]
        [EndpointDescription("Get Role List")]
        [EndpointName("GetRoleList")]
        public ActionResult<ApiResponse> GetRoleList()
        {
            var result = _dropdown.GetRoleList();
            if (result != null)
                return Ok(new ApiResponse(true, "Role List", result.Select(r => new {
                    r.Id,
                    r.RoleName,
                    r.Description,
                    r.IsActive
                })
                    ));

            return Ok(new ApiResponse(false, "Error While Get Role List", null));
        }
        #endregion

        #region Get Content type DropDown List
        [HttpGet("GetContentTypeList")]
        [EndpointSummary("GetContentTypeList")]
        [EndpointDescription("Get Content Type List")]
        [EndpointName("GetContentTypeList")]
        public ActionResult<ApiResponse> GetContentTypeList()
        {
            var result = _dropdown.GetEnumListByType((int)EnumTypes.ContentType);
            if (result != null)
                return Ok(new ApiResponse(true, "Content Type List", result.Select(r => new {
                    r.Id,
                    r.EnumValue,
                    r.Description,
                    r.IsActive
                })
                    ));

            return Ok(new ApiResponse(false, "Error While Get Role List", null));
        }
        #endregion

        #region Get Content DropDown List
        [HttpPost("GetContent")]
        [EndpointSummary("GetContent")]
        [EndpointDescription("Get Content (FNQ,  About Us, Privacy Policy, Terms & Conditions)")]
        [EndpointName("GetContent")]
        public ActionResult<ApiResponse> GetContent(CommonModel model)
        {
            var result = _content.GetContentManagementsContentByID(model.Id);
            if (result != null)
            {
                var response = new
                {
                    id = result.PageId,
                    Content = result.Content
                };
                return Ok(new ApiResponse(true, "Sucess", response));
            }

            return Ok(new ApiResponse(false, "Error While Get Content Data", null));
        }
        #endregion
    }
}
