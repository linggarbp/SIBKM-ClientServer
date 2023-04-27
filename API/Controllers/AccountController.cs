﻿using API.Models;
using API.Repositories.Interface;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        var accounts = _accountRepository.GetAll();
        if(accounts == null)
            return NotFound(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Errors = "Data not Found"
            });

        return Ok(new ResponseDataVM<IEnumerable<Account>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = accounts
        });
    }

    [HttpGet("{id}")]
    public ActionResult GetById(string id)
    {
        var account = _accountRepository.GetById(id);
        if (account == null)
            return NotFound(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Errors = "Id not Found"
            });

        return Ok(new ResponseDataVM<Account>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success",
            Data = account
        });
    }

    [HttpPost]
    public ActionResult Insert(Account account)
    {
        if (account.EmployeeNIK.ToLower() == "string" || account.Password.ToLower() == "string" || account.EmployeeNIK == "" || account.Password == "")
            return BadRequest(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Errors = "Value Cannot be Null or Default"
            });

        var insert = _accountRepository.Insert(account);
        if (insert > 0)
            return Ok(new ResponseDataVM<Account>
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
    public ActionResult Update(Account account)
    {
        if (account.EmployeeNIK.ToLower() == "string" || account.Password.ToLower() == "string" || account.EmployeeNIK == "" || account.Password == "")
            return BadRequest(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Errors = "Value Cannot be Null or Default"
            });

        var update = _accountRepository.Update(account);
        if (update > 0)
            return Ok(new ResponseDataVM<Account>
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
    public ActionResult Delete(string id)
    {
        var account = _accountRepository.GetById(id);
        if (account == null)
            return NotFound(new ResponseErrorVM<string>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Errors = "Id not Found"
            });

        var delete = _accountRepository.Delete(id);
        if (delete > 0)
            return Ok(new ResponseDataVM<Account>
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
