using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;
    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        var accountRoles = _accountRoleRepository.GetAll();
        if (accountRoles == null)
            return NotFound(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Errors = "Data not Found"
            });

        return Ok(new ResponseDataVM<IEnumerable<AccountRole>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = accountRoles
        });
    }

    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        var accountRole = _accountRoleRepository.GetById(id);
        if (accountRole == null)
            return NotFound(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Errors = "Id not Found"
            });

        return Ok(new ResponseDataVM<AccountRole>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = accountRole
        });
    }

    [HttpPost]
    public ActionResult Insert(AccountRole accountRole)
    {
        if (accountRole.AccountNIK == "" || accountRole.AccountNIK.ToLower() == "string" || accountRole.RoleID == 0 || accountRole.RoleID == null)
            return BadRequest(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Errors = "Value Cannot be Null or Default"
            });

        var insert = _accountRoleRepository.Insert(accountRole);
        if (insert > 0)
            return Ok(new ResponseDataVM<AccountRole>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Insert Success",
                Data = null!
            });

        return BadRequest(new ResponseErrorVM<string>
        {
            Code = StatusCodes.Status500InternalServerError,
            Status = HttpStatusCode.InternalServerError.ToString(),
            Errors = "Insert Failed / Lost Connection"
        });
    }

    [HttpPut]
    public ActionResult Update(AccountRole accountRole)
    {
        if (accountRole.AccountNIK == "" || accountRole.AccountNIK.ToLower() == "string" || accountRole.RoleID == 0 || accountRole.RoleID == null)
            return BadRequest(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Errors = "Value Cannot be Null or Default"
            });

        var update = _accountRoleRepository.Update(accountRole);
        if (update > 0)
            return Ok(new ResponseDataVM<AccountRole>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Update Success",
                Data = null!
            });
        return BadRequest(new ResponseErrorVM<string>
        {
            Code = StatusCodes.Status500InternalServerError,
            Status = HttpStatusCode.InternalServerError.ToString(),
            Errors = "Update Failed / Lost Connection"
        });
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var accountRole = _accountRoleRepository.GetById(id);
        if (accountRole == null)
            return NotFound(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Errors = "Id not Found"
            });

        var delete = _accountRoleRepository.Delete(id);
        if (delete > 0)
            return Ok(new ResponseDataVM<AccountRole>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Delete Success",
                Data = null!
            });

        return BadRequest(new ResponseErrorVM<string>
        {
            Code = StatusCodes.Status500InternalServerError,
            Status = HttpStatusCode.InternalServerError.ToString(),
            Errors = "Delete Failed / Lost Connection"
        });
    }
}