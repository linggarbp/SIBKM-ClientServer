using API.Context;
using API.Handler;
using API.Models;
using API.Repositories.Interface;
using API.ViewModels;

namespace API.Repositories.Data;

public class AccountRepository : GeneralRepository<Account, string, MyContext>, IAccountRepository
{
    public AccountRepository(MyContext context) : base(context)
    {
    }

    public int Register(RegisterVM registerVM)
    {
        int result = 0;
        // Insert to University Table
        var university = new University
        {
            Name = registerVM.UniversityName
        };

        //Before Insert, Check University Id
        if (_context.Universities.Any(u => u.Name.Contains(registerVM.UniversityName)))
        {
            university.Id = _context.Universities.FirstOrDefault(u => u.Name.Contains(registerVM.UniversityName))!.Id;
        }
        else
        {
            _context.Set<University>().Add(university);
            result = _context.SaveChanges();
        }

        // Insert to Education Table
        var education = new Education
        {
            Major = registerVM.Major,
            Degree = registerVM.Degree,
            GPA = registerVM.GPA,
            UniversityID = university.Id
        };
        _context.Set<Education>().Add(education);
        result += _context.SaveChanges();

        // Insert to Employee Table
        var employee = new Employee
        {
            NIK = registerVM.NIK,
            FirstName = registerVM.FirstName,
            LastName = registerVM.LastName,
            Gender = registerVM.Gender,
            BirthDate = registerVM.BirthDate,
            Email = registerVM.Email,
            HiringDate = DateTime.Now,
            PhoneNumber = registerVM.PhoneNumber,
        };
        _context.Set<Employee>().Add(employee);
        result += _context.SaveChanges();

        // Insert to Account Table
        var account = new Account
        {
            EmployeeNIK = registerVM.NIK,
            Password = Hashing.HashPassword(registerVM.Password),
        };
        _context.Set<Account>().Add(account);
        result += _context.SaveChanges();

        // Insert to Profiling Table
        var profiling = new Profiling
        {
            EmployeeNIK = registerVM.NIK,
            EducationID = education.ID
        };
        _context.Set<Profiling>().Add(profiling);
        result += _context.SaveChanges();

        // Insert to AccountRole Table
        var accountRole = new AccountRole
        {
            AccountNIK = registerVM.NIK,
            RoleID = 1
        };
        _context.Set<AccountRole>().Add(accountRole);
        result += _context.SaveChanges();

        return result;
    }

    public bool Login(LoginVM loginVM)
    {
        //var employeeByEmail = _context.Employees.FirstOrDefault(e => e.Email == loginVM.Email);
        //if (employeeByEmail == null)
        //    return false;

        //var accountByNIK = _context.Accounts.FirstOrDefault(e => e.EmployeeNIK == employeeByEmail.NIK);

        //return accountByNIK != null && loginVM.Password == accountByNIK.Password;

        var getEmployeeAccount = _context.Employees.Join(_context.Accounts,
                                                                e => e.NIK,
                                                                p => p.EmployeeNIK, (e, p) => new
                                                                {
                                                                    Email = e.Email,
                                                                    Password = p.Password,
                                                                }).FirstOrDefault(e => e.Email == loginVM.Email);

        if (getEmployeeAccount == null)
            return false;

        return Hashing.ValidatePassword(loginVM.Password, getEmployeeAccount.Password);
    }
}
